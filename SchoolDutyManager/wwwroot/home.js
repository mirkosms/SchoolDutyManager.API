document.addEventListener('DOMContentLoaded', function () {
    const token = sessionStorage.getItem('token');
    if (!token) {
        window.location.href = 'index.html';
    }

    // Decode token to get user role
    const payload = JSON.parse(atob(token.split('.')[1]));
    const userRole = payload.role;

    console.log('User role:', userRole);

    const resultDiv = document.getElementById('result');
    const userIdInput = document.getElementById('userId');

    // Function to display users
    function displayUsers(users) {
        resultDiv.innerHTML = '';
        users.forEach(user => {
            const userDiv = document.createElement('div');
            userDiv.textContent = `ID: ${user.id}, Name: ${user.name}, Email: ${user.email}, Roles: ${user.roles.join(', ')}`;
            resultDiv.appendChild(userDiv);
        });
    }

    // Fetch all students
    document.getElementById('getAllStudents').addEventListener('click', function () {
        fetch('https://localhost:5001/Users/role/Student', {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${token}`
            }
        })
            .then(response => response.json())
            .then(data => {
                displayUsers(data);
            })
            .catch(error => console.error('Error fetching students:', error));
    });

    // Fetch all teachers
    document.getElementById('getAllTeachers').addEventListener('click', function () {
        fetch('https://localhost:5001/Users/role/Teacher', {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${token}`
            }
        })
            .then(response => response.json())
            .then(data => {
                displayUsers(data);
            })
            .catch(error => console.error('Error fetching teachers:', error));
    });

    // Fetch user by ID
    document.getElementById('getUserById').addEventListener('click', function () {
        const id = userIdInput.value;
        if (!id) {
            alert('Please enter a user ID');
            return;
        }
        fetch(`https://localhost:5001/Users/${id}`, {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${token}`
            }
        })
            .then(response => response.json())
            .then(data => {
                displayUsers([data]);
            })
            .catch(error => console.error('Error fetching user by ID:', error));
    });

    // Event listener for logout
    document.getElementById('logout').addEventListener('click', function () {
        sessionStorage.removeItem('token');
        window.location.href = 'index.html';
    });

    // Event listener for home
    document.getElementById('home').addEventListener('click', function () {
        window.location.href = 'home.html';
    });
});
