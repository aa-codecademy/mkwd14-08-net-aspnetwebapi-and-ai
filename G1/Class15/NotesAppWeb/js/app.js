// The entry point. This is the only file index.html names, and everything else
// arrives because this one imports it.
//
// It does three things: say what the routes are, say what "logged in" means for
// each of them, and start.

import { isLoggedIn, clearToken, getCurrentUserName, setOnSessionExpired } from "./api.js";
import { startRouter, navigate } from "./router.js";
import { showLoginView } from "./login.js";
import { showNotesView, clearNotesView } from "./notes.js";

// A message waiting to be shown the next time the login screen appears.
// It cannot be passed straight to showLoginView(), because changing the hash
// fires "hashchange" LATER, not immediately - the router would render the login
// view after us and wipe the message off the screen.
let pendingLoginMessage = "";

// The route table. redirectTo is the guard: return a hash to go there instead,
// or null to allow it.
const routes = {
    "#/login": {
        element: "loginView",
        showNavbar: false,
        // Already logged in? There is nothing to do on a login screen.
        redirectTo: () => (isLoggedIn() ? "#/notes" : null),
        show: () => {
            const message = pendingLoginMessage;
            pendingLoginMessage = "";

            showLoginView(message);
        }
    },
    "#/notes": {
        element: "notesView",
        showNavbar: true,
        // The guard. Note what it does NOT do: it keeps the screen hidden, it
        // does not keep the data safe. Anyone can type #/notes, or edit
        // isLoggedIn() in dev tools - and still get nothing, because every
        // request needs a token the API checks for itself.
        redirectTo: () => (isLoggedIn() ? null : "#/login"),
        show: () => {
            document.getElementById("currentUser").textContent = getCurrentUserName();
            showNotesView();
        }
    }
};

// api.js has no way to reach the UI on its own - we give it one. When a token
// dies mid-session this is what runs.
setOnSessionExpired(function () {
    clearNotesView();

    pendingLoginMessage = "Your session expired. Please log in again.";
    navigate("#/login");
});

document.getElementById("logoutButton").addEventListener("click", function () {
    clearToken();
    clearNotesView();

    navigate("#/login");
});

startRouter(routes, "#/login");
