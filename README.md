<img src="https://raw.githubusercontent.com/daskza19/InnovamatQuiz/main/Readme%20files/Titol.png">

<img src="https://raw.githubusercontent.com/daskza19/InnovamatQuiz/main/Readme%20files/intro.png">

Per al contingut d’aquest exercici es demana una aplicació funcional capaç de mostrar un número escrit i, entre les diferents possibilitats del mateix nombre escrit amb valor numèric, escollir l’opció correcta.

<img src="https://raw.githubusercontent.com/daskza19/InnovamatQuiz/main/Readme%20files/desenvolupament.png">

En primer lloc, es va tenir que crear uns diccionaris per guardar la informació de totes les possibles respostes. Es va escollir aquesta opció ja que els diccionaris poden guardar dos valors: el primer el fem utilitzar com a valor numeric (key) i al segon guardem una string amb el mateix número escrit amb paraula.

Seguit es van crear tots els elements necessaris de la UI per fer útil aquesta pantalla. Més tard, es va fer una funció que agafa un número aleatori posicionat dintre del rang del diccionari actual de possibles respostes. Aquest número s’assigna aleatòriament a un dels tres botons i es marca com a opció correcta, per a les altres dos opcions es completen amb altres números aleatoris del diccionari.

Així doncs, quan el jugador pren un dels tres botons es mira si aquest és l’opció correcta o no. Si és l’opció correcta es reinicia el bucle amb valors nous. Si l’opció que ha marcat el jugador és una de les dos incorrectes es marca l’opció en roig i es suma una unitat a un contador del manager. Si aquest contador arriba a dos, es mostra quina era la resposta correcta i es canvia de pregunta un altre cop. Amb tot això, es va guardant a un contador el nombre de respostes encertades i errades.

Per acabar amb els requisits de la prova, es va haver de fer una nova pantalla on es mostrés el número actual de la ronda escrit. Aquest apareixeria al principi de cada ronda. Seguit es va fer un menú principal del joc que s’inicia al principi de l’aplicació i que et permet començar amb el bucle de preguntes o sortir de l’aplicació. Per acabar amb la interacció del menú principal, es va afegir una fletxa a un costat de la pantalla de preguntes per poder tornar al menú.

<img src="https://raw.githubusercontent.com/daskza19/InnovamatQuiz/main/Readme%20files/Foto%2001.png">

Amb tot això es va decidir posar totes les pantalles juntes dintre d’una escena i fer que anessin apareixent dintre de la pantalla quan fos pertinent. Així doncs el canvas es divideix per pantalles que inicialment es troben fora de pla i els elements entren quan es vol mostrar la pantalla.

Per afegir al contingut de la prova es va decidir implementar nivells. Aquests nivells determinarien amb quin rang de números es mostren les preguntes. Es va crear la nova pantalla que es col·locaria entre el menú principal i la pantalla de preguntes. Per seleccionar els rangs es van fer tres diccionaris diferents i quan el jugador escolleix un nivell es determina quin diccionari utilitzar per preguntar.

Al final de tot es van afegir les animacions i procurar que el jugador sol pugui interaccionar amb els botons quan es necessiti. També es va afegir un fons en moviment.

<img src="https://raw.githubusercontent.com/daskza19/InnovamatQuiz/main/Readme%20files/testing.png">

Durant el procés de creació de l’aplicació es va anar fent processos de busca d’errors i/o coses a millorar. Seguit es mostrarà una petita llista amb alguns elements i com es van solucionar:

* No hi havia cap opció per sortir fàcil de l’aplicació.
Degut a que al menú principal no hi havia l’opció de sortir.
Es va arreglar implementant-se el botó adequat.
* L’aplicació semblava que es quedés penjada.
Degut a que no hi havien animacions per pantalla, i durant el procés mental de decidir la resposta correcta no hi havia ningún element que mostri algo funcionant.
Es va arreglar ficant animacions als botons. En primer lloc es va fer els botons movent-se fent petits salts. No obstant, es va decidir canviar d’animació ja que es va pensar que això podria dificultar l’acció de prémer el botó a una part de la població. Es va decidir fer un quadre darrere que anés movent-se. També es va implementar el fons en moviment.
* Es podia clicar dos cops a una mateixa resposta i aquesta conta dos errors.
Degut a que els botons no guarden si ja han estat pressionats anteriorment i eviten el doble tab.
Es va decidir no arreglar aquest “error” degut a un aspecte de disseny. Amb la situació que una persona nova no sap que té dos possibilitats per encertar la resposta, clicarà sempre el mateix botó i veurà que no passa res. Això provocarà que la persona pensi que l’aplicació s’ha quedat penjada. No obstant, si fem que el mateix botó no guardi si ha estat clicat anteriorment, fem veure al jugador una resposta al segon cop que clica i al següent torn pensa per ell/a mateix/a que pot clicar per segon cop a un altre lloc, ensenyant així la mecànica.

