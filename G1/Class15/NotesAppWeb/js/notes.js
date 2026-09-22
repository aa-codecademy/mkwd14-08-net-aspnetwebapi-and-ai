// The notes view: load, render, create, edit, delete.

import { PRIORITIES, TAGS } from "./config.js";
import { getNotes, addNote, updateNote, deleteNote } from "./api.js";

const notesContainer = document.getElementById("notesContainer");
const priorityFilter = document.getElementById("priorityFilter");
const loading = document.getElementById("loading");
const emptyState = document.getElementById("emptyState");
const notesError = document.getElementById("notesError");

const noteForm = document.getElementById("noteForm");
const noteModalTitle = document.getElementById("noteModalTitle");
const noteIdInput = document.getElementById("noteId");
const noteTextInput = document.getElementById("noteText");
const notePriorityInput = document.getElementById("notePriority");
const tagCheckboxes = document.getElementById("tagCheckboxes");
const charCount = document.getElementById("charCount");
const modalError = document.getElementById("modalError");
const saveNoteButton = document.getElementById("saveNoteButton");

const deleteNoteText = document.getElementById("deleteNoteText");
const confirmDeleteButton = document.getElementById("confirmDeleteButton");

// Bootstrap modals are objects, not just markup - we open and close them in code.
// bootstrap comes from the CDN <script> tag, so it is a global, not an import.
const noteModal = new bootstrap.Modal(document.getElementById("noteModal"));
const deleteModal = new bootstrap.Modal(document.getElementById("deleteModal"));

// The note the delete dialog is currently asking about.
let noteToDelete = null;

// ===== Wiring, once, when the module loads =====

priorityFilter.addEventListener("change", loadNotes);
document.getElementById("newNoteButton").addEventListener("click", openCreateModal);
noteTextInput.addEventListener("input", updateCharCount);
noteForm.addEventListener("submit", onSaveNote);
confirmDeleteButton.addEventListener("click", onConfirmDelete);

buildTagCheckboxes();

// Called by the router every time #/notes comes on screen.
export function showNotesView() {
    // A fresh start for whoever just logged in - the previous user's filter and
    // error banner must not survive into this session.
    priorityFilter.value = "";
    hideError();

    loadNotes();
}

// Wipes the board on logout, so the next user never sees a flash of these notes.
export function clearNotesView() {
    notesContainer.innerHTML = "";
    emptyState.classList.add("d-none");
    hideError();
}

// ===== Loading and rendering =====

async function loadNotes() {
    hideError();
    loading.classList.remove("d-none");
    notesContainer.innerHTML = "";
    emptyState.classList.add("d-none");

    try {
        const notes = await getNotes(priorityFilter.value);

        renderNotes(notes);
    } catch (error) {
        // A 401 has already bounced us to the login view by now, so there is
        // nothing useful to show for it.
        if (error.status !== 401) {
            showError(error.message);
        }
    } finally {
        loading.classList.add("d-none");
    }
}

function renderNotes(notes) {
    if (notes.length === 0) {
        emptyState.classList.remove("d-none");
        return;
    }

    // Build the whole list as one string, then touch the DOM once.
    notesContainer.innerHTML = notes.map(buildNoteCard).join("");

    // The buttons only exist after the line above ran, so we wire them up now.
    notesContainer.querySelectorAll(".js-edit").forEach(function (button) {
        button.addEventListener("click", function () {
            openEditModal(notes.find(note => note.id === Number(button.dataset.id)));
        });
    });

    notesContainer.querySelectorAll(".js-delete").forEach(function (button) {
        button.addEventListener("click", function () {
            openDeleteModal(notes.find(note => note.id === Number(button.dataset.id)));
        });
    });
}

function buildNoteCard(note) {
    const priority = PRIORITIES[note.priority];

    const tagBadges = note.tags
        .map(tag => '<span class="badge rounded-pill tag-badge tag-' + tag.color + '">' + escapeHtml(tag.name) + '</span>')
        .join("");

    return '' +
        '<div class="col">' +
        '  <div class="card h-100 shadow-sm note-card note-card-' + note.priority + '">' +
        '    <div class="card-body d-flex flex-column">' +
        '      <div class="d-flex justify-content-between align-items-start mb-2">' +
        '        <span class="badge ' + priority.cssClass + '">' + priority.name + '</span>' +
        '        <small class="text-body-secondary">' + formatDate(note.createdDate) + '</small>' +
        '      </div>' +
        '      <p class="card-text flex-grow-1">' + escapeHtml(note.text) + '</p>' +
        '      <div class="d-flex flex-wrap gap-1 mb-3">' + tagBadges + '</div>' +
        '      <div class="d-flex gap-2">' +
        '        <button type="button" class="btn btn-sm btn-outline-primary js-edit" data-id="' + note.id + '">Edit</button>' +
        '        <button type="button" class="btn btn-sm btn-outline-danger js-delete" data-id="' + note.id + '">Delete</button>' +
        '      </div>' +
        '    </div>' +
        '  </div>' +
        '</div>';
}

// The note text comes from a user and goes into innerHTML. Without this, a note
// saying <script>alert(1)</script> would run. Never put raw input into HTML.
function escapeHtml(text) {
    const div = document.createElement("div");
    div.textContent = text;

    return div.innerHTML;
}

// The API sends ISO date strings; toLocaleDateString shows them the reader's way.
function formatDate(isoDate) {
    return new Date(isoDate).toLocaleDateString();
}

// ===== The add / edit modal =====

function buildTagCheckboxes() {
    tagCheckboxes.innerHTML = TAGS.map(tag => '' +
        '<div class="form-check">' +
        '  <input class="form-check-input js-tag" type="checkbox" value="' + tag.id + '" id="tag-' + tag.id + '" />' +
        '  <label class="form-check-label" for="tag-' + tag.id + '">' + tag.name + '</label>' +
        '</div>').join("");
}

function openCreateModal() {
    resetModal();

    noteModalTitle.textContent = "New note";
    noteIdInput.value = "";

    noteModal.show();
}

function openEditModal(note) {
    resetModal();

    noteModalTitle.textContent = "Edit note";

    // The hidden id is what turns the save into a PUT instead of a POST.
    noteIdInput.value = note.id;
    noteTextInput.value = note.text;
    notePriorityInput.value = note.priority;
    updateCharCount();

    // Tick the tags this note already has.
    const tagIds = note.tags.map(tag => tag.id);

    tagCheckboxes.querySelectorAll(".js-tag").forEach(function (checkbox) {
        checkbox.checked = tagIds.includes(Number(checkbox.value));
    });

    noteModal.show();
}

function resetModal() {
    noteForm.reset();
    noteForm.classList.remove("was-validated");
    modalError.classList.add("d-none");
    updateCharCount();
}

function updateCharCount() {
    charCount.textContent = noteTextInput.value.length;
}

async function onSaveNote(event) {
    event.preventDefault();

    modalError.classList.add("d-none");

    if (!noteForm.checkValidity()) {
        noteForm.classList.add("was-validated");
        return;
    }

    // Priority is a <select>, so its value is the STRING "3". The API binds that
    // to the enum happily, but Number() keeps our own JSON honest.
    const note = {
        text: noteTextInput.value.trim(),
        priority: Number(notePriorityInput.value),
        tagIds: [...tagCheckboxes.querySelectorAll(".js-tag:checked")].map(checkbox => Number(checkbox.value))
    };

    saveNoteButton.disabled = true;

    try {
        if (noteIdInput.value) {
            note.id = Number(noteIdInput.value);
            await updateNote(note);
        } else {
            await addNote(note);
        }

        noteModal.hide();
        await loadNotes();
    } catch (error) {
        if (error.status === 401) {
            noteModal.hide();
        } else {
            // Errors stay INSIDE the modal - the note the user typed is still there.
            modalError.textContent = error.message;
            modalError.classList.remove("d-none");
        }
    } finally {
        saveNoteButton.disabled = false;
    }
}

// ===== Delete =====

function openDeleteModal(note) {
    noteToDelete = note;
    deleteNoteText.textContent = note.text;

    deleteModal.show();
}

async function onConfirmDelete() {
    confirmDeleteButton.disabled = true;

    try {
        await deleteNote(noteToDelete.id);

        deleteModal.hide();
        await loadNotes();
    } catch (error) {
        deleteModal.hide();

        if (error.status !== 401) {
            showError(error.message);
        }
    } finally {
        confirmDeleteButton.disabled = false;
    }
}

// ===== The view-level error banner =====

function showError(message) {
    notesError.textContent = message;
    notesError.classList.remove("d-none");
}

function hideError() {
    notesError.classList.add("d-none");
}
