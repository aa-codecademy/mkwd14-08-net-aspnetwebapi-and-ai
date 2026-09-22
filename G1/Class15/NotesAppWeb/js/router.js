// The whole router. Thirty lines, and it is the thing that makes this a single
// page application instead of two HTML files.
//
// The idea: the bit of the URL after the # is ours. The browser never sends it to
// the server and never reloads the page when it changes - it just fires an event.
// We listen for that event and show a different <section>.
//
// This file imports nothing. It does not know what a note or a token is, which is
// why the views can import it without creating a circle.

let routes = {};
let fallbackHash = "";

// Go somewhere. Setting location.hash is what fires "hashchange".
export function navigate(hash) {
    if (location.hash === hash) {
        // Same hash = no event, so nothing would happen. Re-render by hand.
        handleRoute();
        return;
    }

    location.hash = hash;
}

export function startRouter(routeTable, fallback) {
    routes = routeTable;
    fallbackHash = fallback;

    window.addEventListener("hashchange", handleRoute);

    // And once on startup, for whoever arrived with a # already in the URL.
    handleRoute();
}

function handleRoute() {
    const hash = location.hash;

    // No hash at all (someone just opened the site), or one we do not have.
    // Send them to the fallback and put it in the address bar too - the URL
    // should never claim to be somewhere we are not.
    if (!hash || !routes[hash]) {
        navigate(fallbackHash);
        return;
    }

    const route = routes[hash];

    // A guard is just a function that returns where to go INSTEAD, or null to
    // allow it. This is how "you must be logged in" is enforced.
    const redirectTo = route.redirectTo ? route.redirectTo() : null;

    if (redirectTo) {
        navigate(redirectTo);
        return;
    }

    showOnly(route.element);

    // Each route says whether the navbar belongs on screen.
    document.getElementById("navbar").classList.toggle("d-none", !route.showNavbar);

    route.show();
}

function showOnly(elementId) {
    document.querySelectorAll(".view").forEach(function (view) {
        view.classList.toggle("d-none", view.id !== elementId);
    });
}
