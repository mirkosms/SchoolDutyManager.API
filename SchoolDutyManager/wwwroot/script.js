const apiBaseUrl = "https://localhost:5001/api"; // Adres backendu

async function login() {
    const username = document.getElementById('username').value;
    const password = document.getElementById('password').value;

    try {
        console.log(`Attempting to login with: ${username} ${password}`);
        const response = await fetch(`${apiBaseUrl}/auth/login`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ email: username, password: password })
        });

        if (response.ok) {
            const result = await response.json();
            localStorage.setItem('token', result.token);
            document.getElementById('login-section').style.display = 'none';
            document.getElementById('content-section').style.display = 'block';
            loadData();
        } else {
            const errorText = await response.text();
            alert(`Login failed: ${errorText}`);
        }
    } catch (error) {
        alert(`Login failed: ${error.message}`);
    }
}

async function register() {
    const username = document.getElementById('username').value;
    const password = document.getElementById('password').value;

    try {
        console.log(`Attempting to register with: ${username} ${password}`);
        const response = await fetch(`${apiBaseUrl}/auth/register`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ email: username, password: password })
        });

        if (response.ok) {
            alert('Registration successful. You can now log in.');
        } else {
            const errorText = await response.text();
            alert(`Registration failed: ${errorText}`);
        }
    } catch (error) {
        alert(`Registration failed: ${error.message}`);
    }
}

function logout() {
    localStorage.removeItem('token');
    document.getElementById('login-section').style.display = 'block';
    document.getElementById('content-section').style.display = 'none';
}

async function loadData() {
    await loadClasses();
    await loadDuties();
    await loadDutySwaps();
}

async function loadClasses() {
    const response = await fetch(`${apiBaseUrl}/classes`, {
        headers: {
            'Authorization': `Bearer ${localStorage.getItem('token')}`
        }
    });

    if (response.ok) {
        const classes = await response.json();
        const classesDiv = document.getElementById('classes');
        classesDiv.innerHTML = '';
        classes.forEach(c => {
            classesDiv.innerHTML += `<p>${c.name}</p>`;
        });
    }
}

async function loadDuties() {
    const response = await fetch(`${apiBaseUrl}/duties`, {
        headers: {
            'Authorization': `Bearer ${localStorage.getItem('token')}`
        }
    });

    if (response.ok) {
        const duties = await response.json();
        const dutiesDiv = document.getElementById('duties');
        dutiesDiv.innerHTML = '';
        duties.forEach(d => {
            dutiesDiv.innerHTML += `<p>${d.type}: ${d.hours}</p>`;
        });
    }
}

async function loadDutySwaps() {
    const response = await fetch(`${apiBaseUrl}/dutyswaps`, {
        headers: {
            'Authorization': `Bearer ${localStorage.getItem('token')}`
        }
    });

    if (response.ok) {
        const swaps = await response.json();
        const swapsDiv = document.getElementById('duty-swaps');
        swapsDiv.innerHTML = '';
        swaps.forEach(s => {
            swapsDiv.innerHTML += `<p>Original Duty: ${s.originalDutyId}, Requested Duty: ${s.requestedDutyId}</p>`;
        });
    }
}
