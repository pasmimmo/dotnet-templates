document.getElementById('helloForm').addEventListener('submit', async function(event) {
    event.preventDefault();
    const form = event.target;
    const formData = {
        name: form.name.value,
        surname: form.surname.value,
        gender: form.gender.value
        };
    const response = await fetch('/api/hello', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(formData)
    });
    const result = await response.text();
    alert(result);
});