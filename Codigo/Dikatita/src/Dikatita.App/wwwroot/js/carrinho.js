document.addEventListener("DOMContentLoaded", function () {
    const form = document.getElementById("formCliente");
    form.addEventListener("submit", async function (e) {
        e.preventDefault();

        const formData = new FormData(form);

        const response = await fetch(form.action, {
            method: "POST",
            body: formData
        });

        if (response.redirected) {
            window.location.href = response.url; // Redireciona para página de confirmação
        } else {
            const html = await response.text();
            document.getElementById("conteudoModalCliente").innerHTML = html; // Em caso de erro, atualiza modal
        }
    });
});