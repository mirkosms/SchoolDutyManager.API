document.addEventListener('DOMContentLoaded', function () {
    const token = sessionStorage.getItem('token');
    if (!token) {
        window.location.href = 'index.html';
    }

    // Dekoduj token, aby uzyskać rolę użytkownika
    const payload = JSON.parse(atob(token.split('.')[1]));
    const userRole = payload.role;

    console.log('User role:', userRole); // Logowanie roli użytkownika

    const resultDiv = document.getElementById('result');
    const classIdInput = document.getElementById('classId');
    const classNameInput = document.getElementById('className');

    // Funkcja do wyświetlania klas
    function displayClasses(classes) {
        resultDiv.innerHTML = '';
        classes.forEach(classItem => {
            const classDiv = document.createElement('div');
            classDiv.textContent = `ID: ${classItem.id}, Name: ${classItem.name}, Duration: ${classItem.duration} minutes`;
            resultDiv.appendChild(classDiv);
        });
    }

    // Pokaż odpowiednie przyciski w zależności od roli użytkownika
    if (userRole === 'Student' || userRole === 'Teacher' || userRole === 'Admin') {
        document.getElementById('getAll').addEventListener('click', function () {
            fetch('https://localhost:5001/api/classes', {
                method: 'GET',
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            })
                .then(response => response.json())
                .then(data => {
                    displayClasses(data);
                })
                .catch(error => console.error('Error fetching classes:', error));
        });

        document.getElementById('getById').addEventListener('click', function () {
            const id = classIdInput.value;
            if (!id) {
                alert('Please enter a class ID');
                return;
            }
            fetch(`https://localhost:5001/api/classes/${id}`, {
                method: 'GET',
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            })
                .then(response => response.json())
                .then(data => {
                    displayClasses([data]);
                })
                .catch(error => console.error('Error fetching class by ID:', error));
        });
    }

    if (userRole === 'Teacher' || userRole === 'Admin') {
        document.getElementById('addClass').addEventListener('click', function () {
            const name = prompt('Enter class name:');
            const duration = prompt('Enter class duration:');
            if (!name || !duration) {
                alert('Please enter class name and duration');
                return;
            }
            const newClass = { Name: name, Duration: parseInt(duration) };
            fetch('https://localhost:5001/api/classes', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
                body: JSON.stringify(newClass)
            })
                .then(response => response.json())
                .then(data => {
                    alert('Class added successfully');
                    displayClasses([data]);
                })
                .catch(error => console.error('Error adding class:', error));
        });

        document.getElementById('updateClass').addEventListener('click', function () {
            const id = classIdInput.value;
            if (!id) {
                alert('Please enter a class ID');
                return;
            }
            const name = prompt('Enter new class name:');
            const duration = prompt('Enter new class duration:');
            if (!name || !duration) {
                alert('Please enter class name and duration');
                return;
            }
            const updatedClass = { Id: parseInt(id), Name: name, Duration: parseInt(duration) };
            fetch(`https://localhost:5001/api/classes/${id}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
                body: JSON.stringify(updatedClass)
            })
                .then(response => {
                    if (response.ok) {
                        alert('Class updated successfully');
                        return response.json();
                    } else {
                        throw new Error('Error updating class');
                    }
                })
                .then(data => {
                    displayClasses([data]);
                })
                .catch(error => console.error('Error updating class:', error));
        });

        document.getElementById('deleteClass').addEventListener('click', function () {
            const id = classIdInput.value;
            if (!id) {
                alert('Please enter a class ID');
                return;
            }
            fetch(`https://localhost:5001/api/classes/${id}`, {
                method: 'DELETE',
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            })
                .then(response => {
                    if (response.ok) {
                        alert('Class deleted successfully');
                        resultDiv.innerHTML = '';
                    } else {
                        throw new Error('Error deleting class');
                    }
                })
                .catch(error => console.error('Error deleting class:', error));
        });
    }

    // Ukryj przyciski, które nie są dostępne dla danej roli
    if (userRole === 'Student') {
        document.getElementById('addClass').style.display = 'none';
        document.getElementById('updateClass').style.display = 'none';
        document.getElementById('deleteClass').style.display = 'none';
    }
});
