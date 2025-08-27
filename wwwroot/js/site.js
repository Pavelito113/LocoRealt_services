document.addEventListener('DOMContentLoaded', function () {
    document.querySelectorAll('.categories .collapse').forEach(function (collapse) {
        collapse.addEventListener('show.bs.collapse', function () {
            var btn = this.previousElementSibling.querySelector('i.fas');
            if (btn) {
                btn.classList.remove('fa-folder');
                btn.classList.add('fa-folder-open');
            }
        });
        collapse.addEventListener('hide.bs.collapse', function () {
            var btn = this.previousElementSibling.querySelector('i.fas');
            if (btn) {
                btn.classList.remove('fa-folder-open');
                btn.classList.add('fa-folder');
            }
        });
    });

    const toggleBtn = document.querySelector('.astro-filters-toggle');
    const panel = document.getElementById('astroFiltersPanel');
    if (toggleBtn && panel) {
        toggleBtn.addEventListener('click', function () {
            const expanded = toggleBtn.getAttribute('aria-expanded') === 'true';
            toggleBtn.setAttribute('aria-expanded', !expanded);
            panel.classList.toggle('show', !expanded);
        });
    }
});
