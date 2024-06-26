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
    const dutySwapIdInput = document.getElementById('dutySwapId');

    // Function to display duty swaps
    function displayDutySwaps(dutySwaps) {
        resultDiv.innerHTML = '';
        dutySwaps.forEach(dutySwap => {
            const dutySwapDiv = document.createElement('div');
            dutySwapDiv.textContent = `ID: ${dutySwap.id}, Original Duty ID: ${dutySwap.originalDutyId}, Requested Duty ID: ${dutySwap.requestedDutyId}, Status: ${dutySwap.status}, Initiating Student ID: ${dutySwap.initiatingStudentId}, Responding Student ID: ${dutySwap.respondingStudentId}`;
            resultDiv.appendChild(dutySwapDiv);
        });
    }

    // Fetch all duty swaps
    document.getElementById('getAll').addEventListener('click', function () {
        fetch('https://localhost:5001/api/dutySwaps', {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${token}`
            }
        })
            .then(response => response.json())
            .then(data => {
                displayDutySwaps(data);
            })
            .catch(error => console.error('Error fetching duty swaps:', error));
    });

    // Fetch duty swap by ID
    document.getElementById('getById').addEventListener('click', function () {
        const id = dutySwapIdInput.value;
        if (!id) {
            alert('Please enter a duty swap ID');
            return;
        }
        fetch(`https://localhost:5001/api/dutySwaps/${id}`, {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${token}`
            }
        })
            .then(response => response.json())
            .then(data => {
                displayDutySwaps([data]);
            })
            .catch(error => console.error('Error fetching duty swap by ID:', error));
    });

    // Add new duty swap
    document.getElementById('addDutySwap').addEventListener('click', function () {
        const originalDutyId = prompt('Enter original duty ID:');
        const requestedDutyId = prompt('Enter requested duty ID:');
        const initiatingStudentId = prompt('Enter initiating student ID:');
        const respondingStudentId = prompt('Enter responding student ID:');
        if (!originalDutyId || !requestedDutyId || !initiatingStudentId || !respondingStudentId) {
            alert('Please enter all duty swap details');
            return;
        }
        const newDutySwap = {
            OriginalDutyId: parseInt(originalDutyId),
            RequestedDutyId: parseInt(requestedDutyId),
            Status: "Pending",
            InitiatingStudentId: parseInt(initiatingStudentId),
            RespondingStudentId: parseInt(respondingStudentId)
        };
        fetch('https://localhost:5001/api/dutySwaps', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify(newDutySwap)
        })
            .then(response => response.json())
            .then(data => {
                alert('Duty Swap added successfully');
                displayDutySwaps([data]);
            })
            .catch(error => console.error('Error adding duty swap:', error));
    });

    // Approve duty swap
    document.getElementById('approveDutySwap').addEventListener('click', function () {
        const id = dutySwapIdInput.value;
        if (!id) {
            alert('Please enter a duty swap ID');
            return;
        }
        fetch(`https://localhost:5001/api/dutySwaps/${id}/approve`, {
            method: 'PUT',
            headers: {
                'Authorization': `Bearer ${token}`
            }
        })
            .then(response => {
                if (response.ok) {
                    alert('Duty Swap approved successfully');
                    return response.json();
                } else {
                    throw new Error('Error approving duty swap');
                }
            })
            .then(data => {
                displayDutySwaps([data]);
            })
            .catch(error => console.error('Error approving duty swap:', error));
    });

    // Reject duty swap
    document.getElementById('rejectDutySwap').addEventListener('click', function () {
        const id = dutySwapIdInput.value;
        if (!id) {
            alert('Please enter a duty swap ID');
            return;
        }
        fetch(`https://localhost:5001/api/dutySwaps/${id}/reject`, {
            method: 'PUT',
            headers: {
                'Authorization': `Bearer ${token}`
            }
        })
            .then(response => {
                if (response.ok) {
                    alert('Duty Swap rejected successfully');
                    return response.json();
                } else {
                    throw new Error('Error rejecting duty swap');
                }
            })
            .then(data => {
                displayDutySwaps([data]);
            })
            .catch(error => console.error('Error rejecting duty swap:', error));
    });

    // Delete duty swap
    document.getElementById('deleteDutySwap').addEventListener('click', function () {
        const id = dutySwapIdInput.value;
        if (!id) {
            alert('Please enter a duty swap ID');
            return;
        }
        fetch(`https://localhost:5001/api/dutySwaps/${id}`, {
            method: 'DELETE',
            headers: {
                'Authorization': `Bearer ${token}`
            }
        })
            .then(response => {
                if (response.ok) {
                    alert('Duty Swap deleted successfully');
                    resultDiv.innerHTML = '';
                } else {
                    throw new Error('Error deleting duty swap');
                }
            })
            .catch(error => console.error('Error deleting duty swap:', error));
    });

    // Hide buttons not available for the user's role
    if (userRole === 'Student' || userRole === 'Teacher') {
        document.getElementById('deleteDutySwap').style.display = 'none';
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
