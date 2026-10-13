
(function () {
    var rules = {
        length: function (p) { return p.length >= 8; },
        upper: function (p) { return /[A-Z]/.test(p); },
        lower: function (p) { return /[a-z]/.test(p); },
        number: function (p) { return /[0-9]/.test(p); },
        special: function (p) { return /[^A-Za-z0-9\s]/.test(p); }
    };

    document.querySelectorAll('input[data-pw-checklist]').forEach(function (input) {
        var list = document.getElementById(input.getAttribute('data-pw-checklist'));
        if (!list) return;

        var items = list.querySelectorAll('li[data-rule]');
        items.forEach(function (li) {
            li.setAttribute('data-label', li.textContent.trim());
        });

        function update() {
            var value = input.value;
            items.forEach(function (li) {
                var ok = rules[li.getAttribute('data-rule')](value);
                li.className = ok ? 'text-success' : 'text-muted';
                li.textContent = (ok ? '\u2713 ' : '\u2022 ') + li.getAttribute('data-label');
            });
        }

        input.addEventListener('input', update);
        update();
    });
})();