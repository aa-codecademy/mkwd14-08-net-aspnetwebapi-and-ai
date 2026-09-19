// Everything that would change between machines lives here, and nowhere else.
// The one file a student has to edit if their API runs on a different port.

// The Notes API. HTTPS on purpose - see the certificate note in the class plan.
export const API_BASE_URL = "https://localhost:7144"; // NotesApp
// export const API_BASE_URL = "https://notesapi.test"; // when hosted properly uncomment this line

// Where we keep the JWT between page loads. localStorage survives a refresh;
// sessionStorage would not, and the token would be gone on every F5.
export const TOKEN_KEY = "notesapp.token";

// Priority is an ENUM in C#, so it travels as a number (1/2/3), not as a word.
// This map is what turns 3 back into "High" for the user.
export const PRIORITIES = {
    1: { name: "Low", cssClass: "text-bg-secondary" },
    2: { name: "Medium", cssClass: "text-bg-warning" },
    3: { name: "High", cssClass: "text-bg-danger" }
};

// The five seeded tags, copied by hand from the database.
// Yes, this is duplication: add a tag in SQL and this list is wrong. The proper
// fix is a GET /api/tags endpoint - that is the exercise at the end of the class.
export const TAGS = [
    { id: 1, name: "Homework", color: "cyan" },
    { id: 2, name: "Avenga", color: "blue" },
    { id: 3, name: "Healthy", color: "orange" },
    { id: 4, name: "Exercise", color: "green" },
    { id: 5, name: "Urgent", color: "red" }
];
