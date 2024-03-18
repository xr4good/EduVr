
  
  

![](https://gitlab.com/seriousgamesplatform1/gamecomponents/-/raw/main/topBar.png)

  

# GameLab Components

O objetivo deste trabalho é criar um conjunto de components para serem utilizados em jogos educativos. Abaixo é possivel visualizar uma biblioteca de componentes existentes, sua utilização e um manual de como criar novos componentes.

## Dependencias
Esse projeto trabalha junto do SteamVR e necessida que esse componente esteja instalado para funcionar corretamente. 
https://assetstore.unity.com/packages/tools/integration/steamvr-plugin-32647#publisher

Caso o usuário possua conhecimento técnico do funcionamento dos scripts no Unity, é possivel importar alguns dos componentes em um projeto e eliminar a necessidade do SteamVR, o que consequentemente torna a utilização do mesmo inutil em projetos que possuam VR em mente. 

## Como importar componentes em outros projetos
  Todos os componentes estão exportados na pasta EXPORTEDS deste projeto. São arquivos *.unitypackage que podem ser importados em qualquer projeto do Unity e, em sequencia, podem ser adicionados a cena utilizando o prefab contido na pasta principal. 

  ![](https://gitlab.com/seriousgamesplatform1/gamecomponents/-/raw/main/importTutorial.png)


## Como criar componentes
- Para abrir o projeto no Unity, a ferramenta Git deve estar instalada ([download](https://git-scm.com/downloads)) e configurada nas variaveis de ambiente do sistema operacional. O Git é utilizado para baixar internamente algumas dependencias.

- A cena padrão usada para testar componentes é a *Assets\Scenes\Test_Room*. Essa cena já possui o steamVR configurado para o componente ser testado em modo de depuração onde o jogador é movimentado usando as teclas WASD e o mouse controla a camera.

Ao abrir o projeto, todos os componentes existentes estão localizados na pasta *Assets\COMPONENTS*
Crie uma nova pasta com o nome do componente que vai ser desenvolvido. Todos os assets e scripts que você utilizar no seu componente precisam obrigatoriamente estar dentro da pasta deste componente. 

Quando o componente estiver finalizado, é necessário realizar dois passos. 

- **EXPORTAR PREFAB:**  Para exportar o prefab basta ir no menu *Assets>Create>Prefab*. Um prefab em branco vai ser criado na pasta selecionada atualmente no editor. Nomeio o seu prefab como o nome do seu componente para permitir uma facil identificação e, em seguida, copie todos os objetos pertencentes ao seu componente na cena, dê um duplo clique no prefab e cole nessa nova cena os componentes. Basta salvar e seu prefab vai estar criado contendo os componentes que você colocou dentro dele. 

   ![](https://gitlab.com/seriousgamesplatform1/gamecomponents/-/raw/main/createPrefab.png)

- **EXPORTAR PACOTE:** Para exportar o pacote contendo o prefab e os assets do componentem basta ir no menu *Assets>Export Package*. Na tela que abrir, é só selecionar a pasta do seu componente (não selecione mais nada alem dela) e então exportar para a pasta *Assets\EXPORTEDS*.
Prontom seu pacote está pronto para ser utilizado em outros projetos.


## Biblioteca

### Slider

O slider permite que o jogador interaja com uma tela feita para apresentação de slides. Ele possui dois botões, para voltar e avançar, e tem um feedback visual para alertar quando o primeiro ou o ultimo slide estão sendo visualizados.

 ![](https://gitlab.com/seriousgamesplatform1/gamecomponents/-/raw/main/slideExample.png)
##### Utilização
Basta arrastar para a cena, posicionar onde desejado e escolher as imagens dos slides. 

As imagens dos slides podem ser alteradas no objeto *slideView*, array *slides*. O componente automaticamente reconhece quantos slides são e exibe todos. 
##### Sugestão de Melhoria
Hoje os slides são pegos localmente. Uma sugestão de melhoria seria permitir obter esses slides de uma URL.