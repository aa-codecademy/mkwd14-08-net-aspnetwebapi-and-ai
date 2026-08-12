fetch('https://localhost:7083/api/notes', {
    method: 'POST',
    body: "Get my dog for a walk!"
})
    .then(response => response.json())
    .then(data => console.log('Success:', data))
    .catch(error => console.error('Error:', error));


setTimeout(function () {
    fetch('https://localhost:7083/api/notes')
        .then(response => response.json())
        .then(data => console.log('Notes:', data))
}, 2000)