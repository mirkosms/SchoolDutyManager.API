﻿document.addEventListener('DOMContentLoaded', function () {
    const token = sessionStorage.getItem('token');
    if (!token) {
        window.location.href = 'index.html';
    }

    // Decode token to get user role
    const payload = JSON.parse(atob(token.split('.')[1]));
    const userRole = payload.role;

    console.log('User role:', userRole);

    const resultDiv = document.getElementById('result');
    const dutyIdInput = document.getElementById('dutyId');
    const dutyNameInput = document.getElementById('dutyName');

    // Function to display duties
    function displayDuties(duties) {
        resultDiv.innerHTML = '';
        duties.forEach(duty => {
            const dutyDiv = document.createElement('div');
            dutyDiv.textContent = `ID: ${duty.id}, Type: ${duty.type}, Hours: ${duty.hours}, Assigned People: ${duty.assignedPeople.join(', ')}`;
            resultDiv.appendChild(dutyDiv);
        });
    }

    // Fetch all duties
    document.getElementById('getAll').addEventListener('click', function () {
        fetch('https://localhost:5001/api/duties', {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${token}`
            }
        })
            .then(response => response.json())
            .then(data => {
                displayDuties(data);
            })
            .catch(error => console.error('Error fetching duties:', error));
    });

    // Fetch duty by ID
    document.getElementById('getById').addEventListener('click', function () {
        const id = dutyIdInput.value;
        if (!id) {
            alert('Please enter a duty ID');
            return;
        }
        fetch(`https://localhost:5001/api/duties/${id}`, {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${token}`
            }
        })
            .then(response => response.json())
            .then(data => {
                displayDuties([data]);
            })
            .catch(error => console.error('Error fetching duty by ID:', error));
    });

    if (userRole === 'Teacher' || userRole === 'Admin') {
        // Add new duty
        document.getElementById('addDuty').addEventListener('click', function () {
            const type = prompt('Enter duty type:');
            const hours = prompt('Enter duty hours:');
            if (!type || !hours) {
                alert('Please enter duty type and hours');
                return;
            }
            const newDuty = { Type: type, Hours: hours, AssignedPeople: [] };
            fetch('https://localhost:5001/api/duties', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
                body: JSON.stringify(newDuty)
            })
                .then(response => response.json())
                .then(data => {
                    alert('Duty added successfully');
                    displayDuties([data]);
                })
                .catch(error => console.error('Error adding duty:', error));
        });

        // Update duty
        document.getElementById('updateDuty').addEventListener('click', function () {
            const id = dutyIdInput.value;
            if (!id) {
                alert('Please enter a duty ID');
                return;
            }
            const type = prompt('Enter new duty type:');
            const hours = prompt('Enter new duty hours:');
            if (!type || !hours) {
                alert('Please enter duty type and hours');
                return;
            }
            const updatedDuty = { Id: parseInt(id), Type: type, Hours: hours, AssignedPeople: [] };
            fetch(`https://localhost:5001/api/duties/${id}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
                body: JSON.stringify(updatedDuty)
            })
                .then(response => {
                    if (response.ok) {
                        alert('Duty updated successfully');
                        return response.json();
                    } else {
                        throw new Error('Error updating duty');
                    }
                })
                .then(data => {
                    displayDuties([data]);
                })
                .catch(error => console.error('Error updating duty:', error));
        });

        // Delete duty
        document.getElementById('deleteDuty').addEventListener('click', function () {
            const id = dutyIdInput.value;
            if (!id) {
                alert('Please enter a duty ID');
                return;
            }
            fetch(`https://localhost:5001/api/duties/${id}`, {
                method: 'DELETE',
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            })
                .then(response => {
                    if (response.ok) {
                        alert('Duty deleted successfully');
                        resultDiv.innerHTML = '';
                    } else {
                        throw new Error('Error deleting duty');
                    }
                })
                .catch(error => console.error('Error deleting duty:', error));
        });
    }

    // Hide buttons not available for the user's role
    if (userRole === 'Student') {
        document.getElementById('addDuty').style.display = 'none';
        document.getElementById('updateDuty').style.display = 'none';
        document.getElementById('deleteDuty').style.display = 'none';
    }

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
