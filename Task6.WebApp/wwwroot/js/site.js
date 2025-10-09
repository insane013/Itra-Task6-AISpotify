document.addEventListener("DOMContentLoaded", async () => {
    const localeDropdown = document.getElementById("locale-dropdown");
    const localeSearch = document.getElementById("localeSearch");
    const currentLocaleElem = document.getElementById("current-locale");
    const seedInput = document.getElementById("seedInput");
    const likesRange = document.getElementById("likesRange");
    const likesValue = document.getElementById("likesValue");

    let currentLocale = localStorage.getItem("locale") || "en_US";
    let currentSeed = localStorage.getItem("seed") || "";
    let currentLikes = parseFloat(localStorage.getItem("likes") || 5);

    seedInput.value = currentSeed;
    likesRange.value = currentLikes;
    likesValue.textContent = currentLikes.toFixed(1);
    currentLocaleElem.textContent = currentLocale.toUpperCase();

    const res = await fetch("/Home/Locales");
    const locales = await res.json();

    for (const loc of locales) {
        const li = document.createElement("li");
        const a = document.createElement("a");
        a.className = "dropdown-item locale-option";
        a.dataset.locale = loc.code;
        a.textContent = loc.code;
        li.appendChild(a);
        localeDropdown.appendChild(li);
    }

    localeSearch.addEventListener("input", () => {
        const term = localeSearch.value.toLowerCase();
        localeDropdown.querySelectorAll(".locale-option").forEach(opt => {
            opt.style.display = opt.textContent.toLowerCase().includes(term) ? "" : "none";
        });
    });

    async function updateGeneration() {
        localStorage.setItem("locale", currentLocale);
        localStorage.setItem("seed", currentSeed);
        localStorage.setItem("likes", currentLikes);

        const params = new URLSearchParams({
            locale: currentLocale,
            seed: currentSeed,
            likes: currentLikes
        });

        const response = await fetch(`/Home/Regenerate?${params}`);
        const html = await response.text();

        const container = document.getElementById("songsContainer");
        if (container) {
            container.style.opacity = 0.4;
            container.innerHTML = html;
            setTimeout(() => container.style.opacity = 1, 150);
        }
    }

    localeDropdown.addEventListener("click", e => {
        if (!e.target.classList.contains("locale-option")) return;
        currentLocale = e.target.dataset.locale;
        currentLocaleElem.textContent = currentLocale.toUpperCase();
        updateGeneration();
    });

    seedInput.addEventListener("input", e => {
        currentSeed = e.target.value;
        updateGeneration();
    });

    likesRange.addEventListener("input", e => {
        currentLikes = parseFloat(e.target.value);
        likesValue.textContent = currentLikes.toFixed(1);
        updateGeneration();
    });
});