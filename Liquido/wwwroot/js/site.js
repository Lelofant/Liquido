function updateCartBadge(count) {
    const badge = document.getElementById('cartBadge');
    if (!badge) return;

    if (count > 0) {
        badge.textContent = count;
        badge.style.display = 'inline';
    } else {
        badge.style.display = 'none';
    }
}

function showToast(message, type = 'success') {
    const container = document.getElementById('toastContainer');
    if (!container) return;

    const id = 'toast_' + Date.now();
    const icon = type === 'success'
        ? 'bi-check-circle-fill text-success'
        : 'bi-exclamation-circle-fill text-danger';

    container.insertAdjacentHTML('beforeend', `
        <div id="${id}" class="toast align-items-center border-0 shadow" role="alert">
            <div class="d-flex">
                <div class="toast-body d-flex align-items-center gap-2">
                    <i class="bi ${icon}"></i>
                    ${message}
                </div>
                <button type="button"
                        class="btn-close me-2 m-auto"
                        data-bs-dismiss="toast">
                </button>
            </div>
        </div>
    `);

    const toastEl = document.getElementById(id);
    const toast = new bootstrap.Toast(toastEl, { delay: 3000 });
    toast.show();

    toastEl.addEventListener('hidden.bs.toast', () => toastEl.remove());
}

document.addEventListener('DOMContentLoaded', function () {
    document.querySelectorAll('[data-confirm]').forEach(el => {
        el.addEventListener('click', function (e) {
            const message = this.dataset.confirm ||
                'Are you sure you want to delete this?';
            if (!confirm(message)) {
                e.preventDefault();
            }
        });
    });
});