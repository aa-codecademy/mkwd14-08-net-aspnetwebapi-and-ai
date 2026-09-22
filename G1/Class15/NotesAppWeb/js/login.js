// The login view. One job: swap a username and password for a token, then leave.

import { login } from "./api.js";
import { navigate } from "./router.js";

const loginForm = document.getElementById("loginForm");
const usernameInput = document.getElementById("username");
const passwordInput = document.getElementById("password");
const loginButton = document.getElementById("loginButton");
const loginSpinner = document.getElementById("loginSpinner");
const loginButtonText = document.getElementById("loginButtonText");
const loginError = document.getElementById("loginError");

// Wiring up happens ONCE, when the module loads - not every time the view is
// shown. Do it in showLoginView() instead and every visit adds another listener,
// so the third login sends three requests. A classic SPA bug.
loginForm.addEventListener("submit", onSubmit);

// The seeded-account buttons. Classroom only.
document.querySelectorAll(".js-demo-user").forEach(function (button) {
    button.addEventListener("click", function () {
        usernameInput.value = button.dataset.username;
        passwordInput.value = button.dataset.password;
    });
});

// Called by the router every time #/login comes on screen.
export function showLoginView(message) {
    loginForm.reset();
    loginForm.classList.remove("was-validated");

    if (message) {
        showError(message);
    } else {
        hideError();
    }

    usernameInput.focus();
}

async function onSubmit(event) {
    // Without this the browser posts the form and reloads the page - and the
    // fetch below never runs. The single most common mistake on this screen.
    event.preventDefault();

    hideError();

    // Bootstrap's validation styling, driven by the required attributes.
    if (!loginForm.checkValidity()) {
        loginForm.classList.add("was-validated");
        return;
    }

    setBusy(true);

    try {
        await login(usernameInput.value.trim(), passwordInput.value);

        navigate("#/notes");
    } catch (error) {
        // A wrong password is a 401 from the API with a real message in it -
        // "Username or password is incorrect."
        showError(error.message);
    } finally {
        setBusy(false);
    }
}

function setBusy(isBusy) {
    loginButton.disabled = isBusy;
    loginSpinner.classList.toggle("d-none", !isBusy);
    loginButtonText.textContent = isBusy ? "Logging in..." : "Log in";
}

function showError(message) {
    loginError.textContent = message;
    loginError.classList.remove("d-none");
}

function hideError() {
    loginError.classList.add("d-none");
}
