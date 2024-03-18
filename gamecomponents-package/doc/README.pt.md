# Serious Game Components
![](https://gitlab.com/seriousgamesplatform1/gamecomponents-package/-/raw/main/topBar.png)
## Descrição
Esse projeto é focado na criação de diferentes componentes didáticos que ajudam o usuário a criar um ambiente de sala de aula compacto e diversificado dentro da Unity Engine.

[xAPI](https://xapi.com/overview/) é usada para capturar todas as interações do jogador em relação aos componentes propriamente ditos, construindo diferentes declarações xAPI de acordo com a maneira na qual o jogador interage com esses objetos, e então enviando-os para a LRS desejada.

Atualmente, há três componentes disponíveis para uso, sendo estes:

#### Audio Player

- Um componente que pode guardar uma lista de arquivos de áudio e reproduzí-los de acordo com a ordem da mesma. Ele captura quanto tempo o jogador ouviu a um áudio específico e também se o mesmo jogador ouviu todos os áudios.

#### Quiz
- Armazena um conjunto de perguntas e respostas. A quantidade de respostas pode ser especificada pelo usuário, assim como a quantidade de respostas corretas pra cada pergunta. É caputurada a quantidade de tempo que o jogador ficou em uma pergunta específica, se ela foi respondida corretamente ou não, assim como se o jogador respondeu a todas as perguntas.

#### Slides
- Um componente que armazena uma lista de imagens que podem representar qualquer coisa, como por exemplo uma apresentação de slides. Captura quanto tempo o jogador ficou em um slide e se o mesmo visualizou todas as imagens.

#### LRS Information
- Um componente relativamente simples que faz com que as credenciais de autenticação para a LRS sejam generalizadas para uso de todos os componentes, ao invés de atribuí-las individualmente para cada um.
- Note que apenas um script de LRS Information é permitido por cena, qualquer dúplica **será** destruída assim que o jogo começar.

#### LMS Loader
- Um componente que gerencia a comunicação com a XR4Good LMS para os outros componentes, autenticando e baixando os dados de uma lição específica para carregamento de seus respectivos componentes.
- Um LMS Loader não precisa necessariamente existir na mesma cena dos outros componentes para que a informação baixada seja carregada, uma vez que tal informação já tenha sido baixada.
- Note também que apenas um script de LMS Loader é permitido por cena, qualquer dúplica **será** destruída assim que o jogo começar.

## Instalação
![](https://gitlab.com/seriousgamesplatform1/gamecomponents-package/-/raw/main/bottomBar.png)

 - Para abrir e carregar o pacote no Unity, você precisa ter o [Git](https://git-scm.com/downloads) instalado e configurado no seu computador.

 - Vá para a aba Window e abra o **Package Manager**
 
 - Clique o ícone de **+** e então selecione **Add package from git URL**
 
 - Insira a URL desse repositório e então clique em **Add**
 
 - Espere pela conclusão do download e instalação do pacote
 
 - Depois disso, você pode abrir a seção **Samples** e importar quaisquer Assets dos componentes já desenvolvidos para um prefab pré-montado.

## Videos Explicativos

- [Instalação e Implantação - Parte 1 (Inglês)](https://youtu.be/RUUmode2vJI)

- [Instalação e Implantação - Parte 2 (Inglês)](https://youtu.be/CnGQR6OX7aQ)

## Documentos relacionados

- [PIMENTA, André Schneider Guimarães Montresor. Uma arquitetura de modularização de componentes para desenvolvimento de jogos sérios na Unity Engine. 2022. 39 f. Monografia (Graduação em Ciência da Computação) - Instituto de Ciências Exatas e Biológicas, Universidade Federal de Ouro Preto, Ouro Preto, 2022.](http://www.monografias.ufop.br/handle/35400000/4710)

## Utilização
Após a instalação, a utilização é bem simples. Você pode usar os prefabs disponíveis na seção Samples ou então pode criar seu próprio objeto usando os scripts dos componentes disponíveis nesse pacote.

Para usar os componentes, apenas arraste os prefabs para a cena desejada e ajuste as configurações de cada componente no seu respectivo Unity Inspector. As configurações padrão estão configuradas para carregar o conteúdo baixado da LMS pelo LMS Loader automaticamente.
