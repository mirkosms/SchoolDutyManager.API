document.addEventListener('DOMContentLoaded', function () {
    const token = sessionStorage.getItem('token');
    if (!token) {
        window.location.href = 'index.html';
        return;
    }

    // Decode token to get user role
    const payload = JSON.parse(atob(token.split('.')[1]));
    const userRole = payload.role;

    console.log('User role:', userRole);

    const resultDiv = document.getElementById('result');
    const userIdInput = document.getElementById('userId');

    // Show Add User section only for admin
    if (userRole === 'Admin') {
        document.getElementById('add-user').style.display = 'block';
    }

    // Function to display users
    function displayUsers(users) {
        resultDiv.innerHTML = '';
        users.forEach(user => {
            const userDiv = document.createElement('div');
            userDiv.textContent = `ID: ${user.id}, Name: ${user.firstName} ${user.lastName}, Email: ${user.email}, Roles: ${user.roles.join(', ')}`;
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

    // Add user (Admin Only)
    document.getElementById('add-user-form').addEventListener('submit', function (event) {
        event.preventDefault();
        const email = document.getElementById('add-email').value;
        const password = document.getElementById('add-password').value;
        const role = document.getElementById('add-role').value;

        fetch('https://localhost:5001/api/auth/register', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify({ email, password, role })
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    alert('User added successfully');
                } else {
                    alert('Error adding user: ' + data.message);
                }
            })
            .catch(error => console.error('Error adding user:', error));
    });

    // Update profile
    document.getElementById('update-profile-form').addEventListener('submit', function (event) {
        event.preventDefault();
        const firstName = document.getElementById('update-firstName').value;
        const lastName = document.getElementById('update-lastName').value;
        const password = document.getElementById('update-password').value;

        fetch('https://localhost:5001/Users/current', {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify({ firstName, lastName, password })
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    alert('Profile updated successfully');
                } else {
                    alert('Error updating profile: ' + data.message);
                }
            })
            .catch(error => console.error('Error updating profile:', error));
    });

    // Event listener for logout
    document.getElementById('logout').addEventListener('click', function () {
        sessionStorage.removeItem('token');
        window.location.href = 'index.html';
    });

    // Event listener for dashboard
    document.getElementById('dashboard').addEventListener('click', function () {
        window.location.href = 'dashboard.html';
    });

    // Event listeners for navigation
    document.getElementById('classes').addEventListener('click', function () {
        window.location.href = 'classes.html';
    });

    document.getElementById('duties').addEventListener('click', function () {
        window.location.href = 'duties.html';
    });

    document.getElementById('dutySwaps').addEventListener('click', function () {
        window.location.href = 'duty-swaps.html';
    });
});
