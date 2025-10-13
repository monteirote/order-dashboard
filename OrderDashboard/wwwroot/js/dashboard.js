document.addEventListener('DOMContentLoaded', function () {

    const TEMPO_TROCA_PAGINA = 5000;
    const TEMPO_ATUALIZAR_DADOS = 60000; 
    const LIMITE_QUADROS_PEQUENO = 2;    
    const LIMITE_QUADROS_POR_OS = 12;

    const quadrosContainer = document.getElementById('quadros-container');
    const relogioContainer = document.getElementById('relogio');

    let paginas = []; 
    let paginaAtual = 0;
    let slideshowInterval = null;

    function atualizarRelogio() {
        const agora = new Date();
        relogioContainer.textContent = agora.toLocaleTimeString('pt-BR');
    }

    function dividirOSGrandes (listaDeOS) {
        const listaProcessada = [];

        listaDeOS.forEach(os => {
            if (os.quadros.length <= LIMITE_QUADROS_POR_OS) {
                listaProcessada.push(os);
            } else {
                for (let i = 0; i < os.quadros.length; i += LIMITE_QUADROS_POR_OS) {
                    const pedacoDeQuadros = os.quadros.slice(i, i + LIMITE_QUADROS_POR_OS);
                    const numeroDaParte = (i / LIMITE_QUADROS_POR_OS) + 1;

                    const osParte = {
                        ...os, 
                        osNumber: `${os.osNumber} (Parte ${numeroDaParte})`, 
                        quadros: pedacoDeQuadros
                    };
                    listaProcessada.push(osParte);
                }
            }
        });
        return listaProcessada;
    }

    function criarPaginas (listaDeOS) {
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
                const quadroColClass = (os.quadros.length > 6) ? 'col-md-2' : 'col-md-3';
                let imageURL = quadro.imageUrl == null || quadro.imageUrl.trim() === ''
                    ? '/images/assets/sem-foto-adicionada.jpeg'
                    : `/images/savedImages/${quadro.imageUrl}`;

                quadrosHtml += `
                <div class="${quadroColClass} mb-3">
                    <div class="card h-100 bg-dark text-white">
                        <img src="${imageURL}" class="card-img-top" style="height: 150px; object-fit: cover;">
                        <div class="card-body d-flex flex-column">
                            <h5 class="card-title">${quadro.width}cm x ${quadro.height}cm</h5>
                            <p class="card-text flex-grow-1">${quadro.description || ''}</p>
                            <small>${quadro.frameTypeName || 'N/A'} | ${quadro.glassTypeName || 'N/A'}</small>
                        </div>
                    </div>
                </div>
            `;
            });
            quadrosHtml += '</div>';

            const qrCodeContainerId = `qrcode-os-${os.osNumber}`;
            
            osWrapper.innerHTML = `
                <div class="os-header p-3 rounded mb-3 d-flex justify-content-between align-items-center">
                
                    <div>
                        <h3>${os.osNumber}</h3>
                        <p class="lead mb-0"><strong>Cliente:</strong> ${os.customerName} | <strong>Entrega:</strong> ${dueDate}</p>
                    </div>

                    <div class="text-center ml-3">
                        <div id="${qrCodeContainerId}"></div>
                        <h6 class="mt-1" style="font-size: 0.8rem;">Concluir OS</h6>
                    </div>

                </div>

                ${quadrosHtml}
            `;

            quadrosContainer.appendChild(osWrapper);

            // A lógica de geração do QR Code continua a mesma, pois ela encontra o ID
            const urlParaQrCode = `${window.location.origin}/QRCodeInteraction/CompleteFromQR/${os.osNumber.split(' ')[0]}`;
            const qrCodeContainer = document.getElementById(qrCodeContainerId);

            new QRCode(qrCodeContainer, {
                text: urlParaQrCode,
                width: 120, // Diminuí um pouco para caber melhor no header
                height: 120,
                colorDark: "#ffffff",
                colorLight: "#343a40",
                correctLevel: QRCode.CorrectLevel.H
            });
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

            let ordensDeServico = await response.json();

            const listaProcessada = dividirOSGrandes(ordensDeServico);

            if (slideshowInterval) clearInterval(slideshowInterval);

            if (listaProcessada.length > 0) {
                paginas = criarPaginas(listaProcessada);
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