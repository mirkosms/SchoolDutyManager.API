document.getElementById('login-form').addEventListener('submit', async function (event) {
    event.preventDefault();

    const email = document.getElementById('email').value;
    const password = document.getElementById('password').value;

    try {
        const response = await fetch('https://localhost:5001/api/auth/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ email, password }),
        });

        if (response.ok) {
            const data = await response.json();
            sessionStorage.setItem('token', data.Token); // Zapisz token w sessionStorage
            window.location.href = 'dashboard.html'; // Przekierowanie na stronê dashboardu
        } else {
            const error = await response.json();
            console.error('Login failed:', error.message);
            alert('Login failed: ' + error.message);
        }
    } catch (error) {
        console.error('Login error:', error);
        alert('Login error: ' + error.message);
    }
});
