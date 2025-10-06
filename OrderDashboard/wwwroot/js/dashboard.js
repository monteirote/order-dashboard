document.addEventListener('DOMContentLoaded', function () {

    const TEMPO_TROCA_PAGINA = 15000;
    const TEMPO_ATUALIZAR_DADOS = 60000; 
    const LIMITE_QUADROS_PEQUENO = 2;    

    const quadrosContainer = document.getElementById('quadros-container');
    const relogioContainer = document.getElementById('relogio');

    let paginas = []; 
    let paginaAtual = 0;
    let slideshowInterval = null;

    function atualizarRelogio() {
        const agora = new Date();
        relogioContainer.textContent = agora.toLocaleTimeString('pt-BR');
    }

    function criarPaginas(listaDeOS) {
        const paginasMontadas = [];
        let i = 0;
        while (i < listaDeOS.length) {
            const osAtual = listaDeOS[i];

            if (osAtual.quadros.length > LIMITE_QUADROS_PEQUENO) {
                paginasMontadas.push([osAtual]);
                i++; 
                continue;
            }

            const proximaOS = (i + 1 < listaDeOS.length) ? listaDeOS[i + 1] : null;

            if (proximaOS && proximaOS.quadros.length <= LIMITE_QUADROS_PEQUENO) {
                paginasMontadas.push([osAtual, proximaOS]);
                i += 2; 
            } else {
                paginasMontadas.push([osAtual]);
                i++;
            }
        }
        return paginasMontadas;
    }

    function exibirPagina(indicePagina) {
        quadrosContainer.innerHTML = '';
        const osDaPagina = paginas[indicePagina];
        const numOSNestaPagina = osDaPagina.length;

        osDaPagina.forEach(os => {
            const dueDate = new Date(os.dueDate).toLocaleDateString('pt-BR', { day: '2-digit', month: '2-digit', year: 'numeric', hour: '2-digit', minute: '2-digit' });

            const osWrapper = document.createElement('div');
            osWrapper.className = `col-lg-${12 / numOSNestaPagina} mb-4 d-flex flex-column`;

            let quadrosHtml = '<div class="row">';
            os.quadros.forEach(quadro => {
                const quadroColClass = (numOSNestaPagina === 1) ? 'col-md-4' : 'col-md-6';

                let imageURL = `/images/savedImages/${quadro.imageUrl}` || '/images/assets/sem-foto-adicionada.jpeg';

                quadrosHtml += `
                    <div class="${quadroColClass} mb-3">
                        <div class="card h-100 bg-dark text-white">
                            <img src="${imageURL}" class="card-img-top" style="height: 150px; object-fit: cover;">
                            <div class="card-body">
                                <h5 class="card-title">${quadro.width}cm x ${quadro.height}cm</h5>
                                <p class="card-text">${quadro.description || ''}</p>
                                <small>${quadro.frameTypeName || 'N/A'} | ${quadro.glassTypeName || 'N/A'}</small>
                            </div>
                        </div>
                    </div>
                `;
            });
            quadrosHtml += '</div>';

            osWrapper.innerHTML = `
                <div class="os-header p-3 rounded mb-3">
                    <h3>OS: ${os.osNumber}</h3>
                    <p class="lead mb-0"><strong>Cliente:</strong> ${os.customerName} | <strong>Entrega:</strong> ${dueDate}</p>
                </div>
                ${quadrosHtml}
            `;
            quadrosContainer.appendChild(osWrapper);
        });
    }

    function trocarPagina() {
        quadrosContainer.classList.add('fade-out');

        setTimeout(() => {
            paginaAtual++;
            if (paginaAtual >= paginas.length) {
                paginaAtual = 0;
            }
            exibirPagina(paginaAtual);
            quadrosContainer.classList.remove('fade-out');
        }, 500);
    }

    async function atualizarDados() {
        try {
            const response = await fetch('/Dashboard/ObterQuadrosEmProducao');
            if (!response.ok) throw new Error('Falha na resposta da rede');

            const ordensDeServico = await response.json();

            if (slideshowInterval) clearInterval(slideshowInterval);

            if (ordensDeServico.length > 0) {
                paginas = criarPaginas(ordensDeServico);

                if (paginas.length > 0) {
                    paginaAtual = -1;
                    trocarPagina();

                    if (paginas.length > 1) {
                        slideshowInterval = setInterval(trocarPagina, TEMPO_TROCA_PAGINA);
                    }
                }
            } else {
                quadrosContainer.innerHTML = '<div class="col-12"><p class="display-4">Nenhum quadro em produção no momento.</p></div>';
            }
        } catch (error) {
            console.error("Não foi possível buscar os dados:", error);
            quadrosContainer.innerHTML = '<div class="col-12"><p class="text-danger">Erro ao carregar os dados.</p></div>';
        }
    }

    atualizarRelogio();
    setInterval(atualizarRelogio, 1000);

    atualizarDados();
    setInterval(atualizarDados, TEMPO_ATUALIZAR_DADOS);
});