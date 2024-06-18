document.addEventListener('DOMContentLoaded', function () {
    const token = sessionStorage.getItem('token');
    if (!token) {
        window.location.href = 'index.html';
    }

    // Przyciski wylogowania
    document.getElementById('logout-btn').addEventListener('click', function (event) {
        event.preventDefault();
        sessionStorage.removeItem('token');
        window.location.href = 'index.html';
    });

    // Pobieranie informacji o użytkowniku
    fetch('https://localhost:5001/api/auth/userinfo', {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`
        }
    })
        .then(response => response.json())
        .then(data => {
            // Dostosowanie menu w zależności od roli użytkownika
            if (data.roles.includes('Teacher')) {
                const teacherOptions = document.createElement('li');
                teacherOptions.innerHTML = '<a href="teacher-options.html">Teacher Options</a>';
                document.getElementById('menu-options').appendChild(teacherOptions);
            }
        })
        .catch(error => console.error('Error fetching user info:', error));
});
