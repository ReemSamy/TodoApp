document.querySelectorAll('.delete-btn').forEach(btn => {
    btn.addEventListener('click', (e) => {
        e.preventDefault();
        const todoId = btn.dataset.id; 

        const modal = new bootstrap.Modal(document.getElementById('deleteModal'));
        const confirmBtn = document.getElementById('confirmDelete');

        confirmBtn.onclick = () => {
            const csrfToken = document.querySelector('input[name="__RequestVerificationToken"]').value;
            fetch(`/ToDo/Delete/${todoId}`, {
                method: 'POST',
                headers: {
                    'RequestVerificationToken': csrfToken,
                    'Content-Type': 'application/json'
                }
            }).then(response => {
                if (response.ok) {
                    window.location.reload(); 
                }
            });
        };

        modal.show();
    });
});
