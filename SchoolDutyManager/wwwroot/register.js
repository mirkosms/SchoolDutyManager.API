document.getElementById('register-form').addEventListener('submit', async function (event) {
    event.preventDefault();

    const email = document.getElementById('email').value;
    const password = document.getElementById('password').value;
    const role = document.getElementById('role').value;

    try {
        const response = await fetch('https://localhost:5001/api/auth/register', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ email, password, role }),
        });

        const data = await response.json();
        if (response.ok) {
            alert(data.Message); // Pokaż komunikat sukcesu
        } else {
            throw new Error(data.Message);
        }
    } catch (error) {
        console.error('Registration error:', error);
        alert('Registration error: ' + error.message);
    }
});
