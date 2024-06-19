document.addEventListener('DOMContentLoaded', function () {
    const token = sessionStorage.getItem('token');
    if (!token) {
        window.location.href = 'index.html';
    }

    // Decode token to get user role and email
    const payload = JSON.parse(atob(token.split('.')[1]));
    const userRole = payload.role;
    const userEmail = payload.email;

    console.log('User role:', userRole);

    const welcomeMessageDiv = document.getElementById('welcomeMessage');
    const userInfoDiv = document.getElementById('userInfo');
    const resultDiv = document.getElementById('result');

    // Function to display data
    function displayData(data, title) {
        resultDiv.innerHTML = `<h2>${title}</h2>`;
        data.forEach(item => {
            const itemDiv = document.createElement('div');
            itemDiv.textContent = JSON.stringify(item, null, 2);
            resultDiv.appendChild(itemDiv);
        });
    }

    // Fetch user info
    fetch('https://localhost:5001/api/auth/userinfo', {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`
        }
    })
        .then(response => response.json())
        .then(userInfo => {
            welcomeMessageDiv.innerHTML = `<h2>Welcome, ${userInfo.roles.join(', ')}</h2>`;
            userInfoDiv.innerHTML = `
            <p><strong>Email:</strong> ${userInfo.email}</p>
            <p><strong>First Name:</strong> ${userInfo.firstName}</p>
            <p><strong>Last Name:</strong> ${userInfo.lastName}</p>
            <p><strong>Roles:</strong> ${userInfo.roles.join(', ')}</p>
        `;
        })
        .catch(error => console.error('Error fetching user info:', error));

    // Fetch all classes
    document.getElementById('viewClasses').addEventListener('click', function () {
        fetch('https://localhost:5001/api/classes', {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${token}`
            }
        })
            .then(response => response.json())
            .then(data => {
                displayData(data, 'Classes');
            })
            .catch(error => console.error('Error fetching classes:', error));
    });

    // Fetch all duties
    document.getElementById('viewDuties').addEventListener('click', function () {
        fetch('https://localhost:5001/api/duties', {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${token}`
            }
        })
            .then(response => response.json())
            .then(data => {
                displayData(data, 'Duties');
            })
            .catch(error => console.error('Error fetching duties:', error));
    });

    // Fetch all duty swaps
    document.getElementById('viewSwaps').addEventListener('click', function () {
        fetch('https://localhost:5001/api/dutySwaps', {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${token}`
            }
        })
            .then(response => response.json())
            .then(data => {
                displayData(data, 'Duty Swaps');
            })
            .catch(error => console.error('Error fetching duty swaps:', error));
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
