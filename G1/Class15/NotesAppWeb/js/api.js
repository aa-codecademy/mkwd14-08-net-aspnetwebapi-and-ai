// Every call to the Notes API goes through this file. The views never see a URL,
// a header or a status code - they call getNotes() and get notes.
//
// There is not one line of DOM code below. That is deliberate: this file could be
// dropped into a console app, a test, or a React project unchanged.

import { API_BASE_URL, TOKEN_KEY } from "./config.js";

// ===== The token =====

export function saveToken(token) {

}

export function getToken() {

}

export function clearToken() {

}

// Reads the payload out of the JWT. Base64, not encryption - open any token on
// jwt.io and you can read it too. That is exactly why nothing secret goes in one.
function readTokenPayload() {
    const token = getToken();

    if (!token) {
        return null;
    }

    try {
        // header.payload.signature - we want the middle part.
        const payload = token.split(".")[1];

        // JWT uses base64URL: - and _ instead of + and /, so swap them back.
        const json = atob(payload.replace(/-/g, "+").replace(/_/g, "/"));

        return JSON.parse(json);
    } catch {
        // A corrupted token is the same as no token.
        return null;
    }
}

// "fullName" is our own custom claim, added in AuthService.GenerateToken().
export function getCurrentUserName() {
    const payload = readTokenPayload();

    return payload ? payload.fullName : "";
}

// exp is in SECONDS since 1970, Date.now() is in MILLISECONDS. Forget the * 1000
// and every token looks expired.
function isTokenExpired() {
    const payload = readTokenPayload();

    if (!payload || !payload.exp) {
        return true;
    }

    return payload.exp * 1000 < Date.now();
}

// The single question the router asks before letting anyone near the notes view.
export function isLoggedIn() {
    return Boolean(getToken()) && !isTokenExpired();
}

// ===== Telling the app the session died =====

// This file must not know that a login screen exists - so it does not go and find
// one. app.js hands us a function, and this is the only way out of here.
// Without it, api.js would have to import the router, the router imports the
// views, and the views import api.js: a circle that ES modules will not resolve.
let onSessionExpired = function () { };

export function setOnSessionExpired(callback) {
    onSessionExpired = callback;
}

// ===== The one function that actually talks to the API =====

// An error that still knows the HTTP status, so callers can treat 403 differently
// from 500 without parsing strings.
export class ApiError extends Error {
    constructor(message, status) {
        super(message);
        this.status = status;
    }
}

// anonymous: true marks a call that is allowed to fail with 401 - see below.
async function apiFetch(path, options = {}) {


}

// Our API answers every failure with application/problem+json, so the message a
// user sees comes straight from Problem(detail: ...) in the controller.
async function readProblemDetail(response) {
    try {
        const problem = await response.json();

        // [ApiController] model-validation errors look different: an "errors"
        // object with one array of messages per field.
        if (problem.errors) {
            return Object.values(problem.errors).flat().join(" ");
        }

        return problem.detail || problem.title || "Request failed.";
    } catch {
        // Not every failure is JSON - a 502 from a proxy is usually HTML.
        return "Request failed with status " + response.status + ".";
    }
}

// ===== The endpoints, one function each =====

// POST /api/auth/login
export async function login(username, password) {

}

// GET /api/notes  /  GET /api/notes?priority=3
export async function getNotes(priority) {
    const query = priority ? "?priority=" + priority : "";

    return await apiFetch("/api/notes" + query);
}

// POST /api/notes - no userId in the body, the API takes the owner from the token.
export async function addNote(note) {
    return await apiFetch("/api/notes", {
        method: "POST",
        body: JSON.stringify(note)
    });
}

// PUT /api/notes - the id goes in the BODY here, not in the URL.
export async function updateNote(note) {
    return await apiFetch("/api/notes", {
        method: "PUT",
        body: JSON.stringify(note)
    });
}

// DELETE /api/notes/5
export async function deleteNote(id) {
    return await apiFetch("/api/notes/" + id, { method: "DELETE" });
}
