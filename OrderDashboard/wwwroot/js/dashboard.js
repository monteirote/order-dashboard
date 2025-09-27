document.addEventListener('DOMContentLoaded', function () {

    const quadrosContainer = document.getElementById('quadros-container');
    const relogioContainer = document.getElementById('relogio');

    function atualizarRelogio() {
        const agora = new Date();
        relogioContainer.textContent = agora.toLocaleTimeString('pt-BR');
    }

    async function atualizarQuadros() {
        try {
            const response = await fetch('/Dashboard/ObterQuadrosEmProducao');
            if (!response.ok) {
                throw new Error(`Erro HTTP! status: ${response.status}`);
            }
            const quadros = await response.json();

            console.log(quadros);

            quadrosContainer.innerHTML = '';

            if (quadros.length === 0) {
                quadrosContainer.innerHTML = '<p class="col-12">Nenhum quadro em produção no momento.</p>';
                return;
            }

            quadros.forEach((quadro, index) => {

                console.log(quadro.imageUrl);

                const quadroHtml = `
                    <div class="col-12 col-md-6 col-lg-4 mb-4">
                        <div class="card h-100">
                            <img src="${quadro.imageUrl}" class="card-img-top">
                            <div class="card-body">
                                <h5 class="card-title">Quadro ${quadro.height / 100} x ${quadro.width / 100} </h5>
                                <h6 class="card-subtitle mb-2 text-muted">Materiais:</h6>
                                <ul>
                                    <li>Data de Entrega: 10/01/2023</li>
                                    <li>Moldura: ${quadro.frame}</li>
                                    <li>Vidro: ${quadro.glassType}</li>
                                </ul>
                                <p>blallblblallflvblasldlasldasldalsdlsaladsldsaldlsaldasldlaslds</p>
                            </div>
                        </div>
                    </div>
                `;
                quadrosContainer.innerHTML += quadroHtml;
            });

        } catch (error) {
            console.error("Não foi possível buscar os dados dos quadros:", error);
            quadrosContainer.innerHTML = '<p class="col-12 text-danger">Erro ao carregar os dados. Verifique o console.</p>';
        }
    }

    atualizarRelogio();
    atualizarQuadros();

    setInterval(atualizarRelogio, 1000);

    setInterval(atualizarQuadros, 10000);
});