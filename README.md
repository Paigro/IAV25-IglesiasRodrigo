# IAV25-IglesiasRodrigo

## Info:

Proyecto final de la asignatura de Inteligencia Artificial de Videojuegos del GdV UCM.

[Pablo Iglesias Rodrigo](https://github.com/Paigro)

## Resumen:

Partida de La Escoba entre 2 IAs con modelos diferentes de IA.

## Idea:

Juego de cartas de la baraja española de *La Escoba* en Unity. La idea es hacer 2 IAs que jueguen entre ellas con la posibilidad de poder tomar el control de una de ellas por el jugador si asi lo desea. Se implementaran 2 modelos de IA: *Monte Carlo Tree Search* y *Utility AI*, y antes de empezar la partida se podrá determinar cual de los dos tendrá cada IA. Luego jugarán n partidas 

## Base:

### La Escoba:

La Escoba es un juego de cartas de la baraja española (40 cartas: 1->12 sin 8s y 9s y las figuras valiendo: sota = 8, caballo = 9 y rey = 10) . Se puede jugar entre 2, 3 o hasta 6 jugadores (yo me he centrado en 1vs1), en el que los jugadores tiene que crear combinaciones de cartas que sumen 15. 

El escenario de juego se compone de mazo, mesa y las diferentes manos y pilas de los jugador.

Al inicio de la partida se reparten 3 cartas a cada jugador y se disponen 4 cartas en la mesa. Luego juegan los jugadores de manera alternativa. Al jugador que le toque mira de sus cartas y busca combinaciones de 15 puntos. Para ello usando solo 1 de sus cartas tiene que encontrar las cartas necesarias de la mesa para sumar 15. Si lo consigue se las lleva a su pila, sino tiene que elegir una de sus cartas y dejarla en la mesa. Cuando los jugadores se quedan sin cartas en la mano. Se reparten otras 3 a cada uno pero ninguna a la mesa. Así sucende la partida hasta que el mazo se queda sin cartas. Las cartas que se quedan en la mesa al acabar tienen que sumar un múltiplo de 5 (sino alguien se ha equivocado) y le pertenecen al último jugador que se haya llevado cartas a su pila. 

Cuando por ejemplo alguien con una carta de su mano y usando todas las de la mesa suma 15, entonces ha hecho una escoba, que es un punto inmediato.

Cuando el mazo se queda sin cartas se contabilizan los puntos de cada jugador. Para ello se mira la pila de cada uno. Hay 5 tipos de puntos: 

1. Un punto para el jugador con más cartas.
2. Un punto para el jugador con más oros.
3. Un punto para el jugador con más sietes.
4. Un punto para el jugador que haya capturado el siete de oros (el velo).
5. Un punto por cada escoba realizada.

En el caso de empate en los 3 primeros nadie suma esos puntos. 

El ciclo de juego se repite hasta que un jugador llegue a (21 + puntos conseguidos por el ganador de la primera partida) puntos o en el caso de que ambos lleguen a esa cantidad el que mayor puntos tenga. En el caso de empate se puede desempatar jugando otra ronda, asi hasta que uno de ellos supere al otro.

Si no queda claro o algo no esta del todo bien porque lo he redactado de cabeza, aquí hay más información: [Wikipedia](https://es.wikipedia.org/wiki/Escoba_del_15) o [nhFournier](https://www.nhfournier.es/como-jugar/escoba/)

### Adaptaciones tomadas:

En el juego de La Escoba hay varias reglas (que se pueden usar o no) como por ejemplo:

1. Escobas de mesa: cuando se reparte al principio la mesa, si el valor de las cartas es 15 o 30 y las cartas pueden formar combinaciones legales de 15, el jugador que reparte se lleva 1 o 2 escobas respectivamente. No lo he implementado, a lo mejor a futuro.
2. Escobas de mano: cuando a un jugador se le reparten las cartas y el valor de ellas suma 15, el jugador puede reclamar escoba de mano que suma 3 puntos pero se queda sin cartas sin jugar y el oponente juega solo hasta que se quede sin cartas. No lo he implementado.
3. La regla base de que los puntos que hay que alcanzar son (21 + puntos del ganador de la primera ronda) no ha sido implementada para no alargar de más posibles partidas pero sería intersante a futuro.

### El proyecto:

La base del proyecto es nula, un proyecto vacío de Unity y mis ganas de hacer uno de mis juegos favoritos de cartas. Recalco, aunque este en referencias, que las imagenes de las cartas (y el oro recortado para el logo) han sido sacados de Itch.io. También ha sido usado un modulo de Tweens de la Asset Store de Unity (también puesto en referencias).

## Planteamiento del problema:

**A** - Creacion del entorno de juego. Creación del tablero, el mazo y manos. El GameManager, UIManager y LevelManager. Los assets serán sacados de Internet si no da tiempo a crearlos.
 
**B** - Crear el modelo de IA MCTS. El modelo va recorriendo un arbol de jugadas dadas las posibles cartas que tiene y hay en la mesa para escoger la mejor jugada.

**C** - Crear el modelo de IA UtilityAI. El modelo asigna pesos a las cartas o jugadas (escoba, 7 de oros, sietes, oros y cartas) y dadas todas las combinaciones, hace la heuristica y se queda con el mejor resultado.

**D** - Testing de partida. Comprobar que el juego se juega, las rondas y partidas van como deberían y no hay errores.

**E** - Usuario jugador (opcional pero casi seguro). Implementar que el jugador pueda tomar el control de una de las IAs (desde el menu entre rondas), tapando las cartas de la otra IA. Tambien se podra devolver el control a la IA.

**F** - Sistema de telemetría (opcional muy MUY opcional). Implementar un sistema de telemetría usando los conocimientos dados en UAJ. Quiza no para IAV pero para la futura continiacion de este proyecto porque siempre he querido hacer mi propia Escoba. Que sobre esto tengo una dura, cuando acabe esta asignatura, este proyecto se puede seguir usando???

## Diseño de la solución:

```
class MCTSNode(state, parent = none, parent_action = none):
  state = state # Represents the state of the game. Cards in hand, cards on table and maybe there is chance to memorice the cards that has been taken by itself and/or oponent.
  parentNode = parent # Reference to previous node.
  parentAction = parent_action # Reference the action taken from the parent node. 
  children: list # Possible actions from this node.
  nVisits: int # Number of visits this node has been visited
  accPoints: int # Number of points accumulated in this node.
  untriedActions: list = state.getPossibleActions() # Remaining actions.

    function expands() -> MCTSNode:
      action = untriedActions.pop() # Pick the first action to make.
      simState = state.move(action) # Simulates the next state base in the action taken.
      newChild = MCTSNode(simState, this, action) # Creates a new child with the new properties.
      children.add(newChild) # Adds the child to the list of childs.
      return newChild

    function simulate() -> float:
      simState = state # Copy of the state to simulate.
      while not simState.isEnough():
        possibleMoves = simState.getPossibleActions()
        action = simulatePolicy(possibleMoves)
        simState = simState.move(action)
      return simState.evaluate()

    function backpropagate(result) -> void
      if this == null:
        return
      nVisits += 1
      accPoints += result
      if parent:
        parent.backpropagate(result)

    function isFullyExpanded() -> bool:
      return (untriedActions.size == 0)

    function select() -> MCTSNode:
      node = this
      while !node.isFullyExpanded() and node.children.count > 0:
        node = node.bestChild(c = √2)

    function bestChild(c) -> MCTSNode:
      return max{child in this.children} ((child.accPoints / child.nVisits) + c * sqrt(ln(this.nVisits) / child.nVisits)) # With UCB1.

  
function isEnough() -> bool:
  return accPoints >= 15

function MCTS(initState, maxTime) -> Action:
  initNode = MCTSNode(initState)
  initTime = getTime()
  while (getTime() - initTime) < maxTime:
    node = select()
    if not node.isFullyExpanded():
      node = expands()
    result = simulate()
    node.backpropagate(result)
  bestChild = max{h in rootNode.children} h.nVisits
  return bestChild.parentAction

  ##----Faltaría definir las simulatePolicies pero son casi iguales a las de Utility AI----##

```

```
CONST BROOM_COST = 0.5 # The cost added when making a Scopa (clean the table).
CONST GOLDEN_SEVEN_COST = 0.4 # The cost added when the golden seven is in the move.
COST SEVEN_COST = 0.3 # The cost added when sevens are in the move.
COST GOLD_COST = 0.2 # The cost added when golds are in the move.
COST CARD_COST = 0.1 # The cost added for every card in the move.

CONST NUM_CARDS = 40
CONST NUM_SEVENS = 4
CONST NUM_GOLDS = 10

class UtilityAI:

  function evaluateMove(move) -> float:
    # parameters to later know witch move is better.
    scopaInMove = 0 # 0 if not, 1 if yes.
    goldenSevenInMove = 0 # 0 if not, 1 if yes.
    sevensInMove = 0 # 0, 1 or 2 depens on the move.
    goldsInMove = 0 # 0, 1, ... depens on the move.
    cardsInMove = 0 # 0, 1,... depends on the move.

    if move.hasScopa():
      scopaInMove = 1
    
    if move.hasGoldenSeven():
      goldenSevenInMove = 1
    
    sevensInMove = move.numSevens() / NUM_SEVENS # The division is to reduce the final result and not get a huge number.
    goldsInMove = move.numGolds() / NUM_GOLDS # Same with the golds.
    cardsInMove = move.numCards() / NUM_CARDS # Same as before.

    # Total calculation of the move.
    total = scopaInMove * BROOM_COST + goldenSevenInMove * GOLDEN_SEVEN_COST + sevensInMove * SEVEN_COST + goldsInMove * GOLD_COST + cardsInMove * CARD_COST
    return total
  
  # Pick the best move of all given moves.
  function getBestMove() -> Move:
    bestMove: Move = null
    bestTotalResult = 0

    for move in possibleMoves:
      result = evaluateMove(move)
      if result > bestTotalResult
        bestMove = move
        bestTotalResult = result

    return bestMove
  
```

## Clases extras:

### Carta:

Struct que contiene la informacion de una carta. Palo y numero. Se usara la baraja española sin 8s ni 9s.

```

struct Card:
  suit: char
  number: int

  Card(s, n):
    suit = s
    number = n

  # For private reasons it has getters.
  function getSuit() -> char
    return suit
  function getNumber() -> int:
    return number

```

### Mazo:

Clase mazo que se encarga de crear el mazo, barajarlo y dar cartas a la mesa o a los jugadores. Tambien se podra leer un mazo de txt para asi facilitar la tarea de testing con mazos y posibles jugadas personalizables por el usuario.

```

class Deck:
  
  cards: list
  nCards: int

  # Constructor.
  Deck():
    cards = []
    createDeck() 
    shuffle(2) # Two times is better than one.

  # Creates a new deck in order.
  function createDeck():
    cards.clear()
    for i = 0 to 3:
       for j = 1 to 10:
         switch(i):
           case 0:
             cards.add(new Card('O', j))
            case 1:
             cards.add(new Card('E', j))
            case 2:
             cards.add(new Card('C', j))
            case 3:
             cards.add(new Card('B', j))
    nCards = 40

  # Shuffle the cards randmoly
  function shuffle(nTimes = 1) -> void:
    # By Fisher-Yates method
    n = cards.size()
    for i = 0 to nTimes:
      for k = n - 1 down to 1:
        k = randomInt(0, j)
        temp = cards[j]
        cards[j] = cards[k]
        cards[k] = temp
  
  # Gives to the player hand 1 card. The management of giving 3 cards each round belogs to other Manager.
  function giveCardToCard(playerHand) -> void:
    playerHand.addCard(cards.pop())
    nCards--
  
  # Add a card to the table. The management of putting 4 cards on the table belogs to other Manager.
  function giveCardToTable(table)
    table.addCard(cards.pop())
    nCards--

  # Resets the deck:
  function reset() -> void:
    createDeck()
    shuffle(2) # Two times is better than one.

  # Read a deck from a txt. To make easier to test things.
  function readDeck() -> void:
    # Read logic.

```

### Mano:

Clase mano que contiene las cartas que tiene el jugador en la mano y se encarga de recibir cartas del mazo y quitarlas de la mano.

```

class Hand:
  
  nCards: int
  cards: list

  Hand():
    cards = []
    nCards = 0

  # Adds a card to the hand.
  function addCard(newCard)
    cards[nCards + 1].add(newCard)
    nCards--

  # Removes a card from the hand.
  function removesCard(card)
    cards.remove(card) # Plus de logic of moving the other cards if card is not the last.
    nCards++

```

### Pila:

Clase que contiene las cartas que ha ido recogiendo cada jugador en una ronda para el recuento final de puntos. Para mayor facilidad llevara tambien la cuenta de escobas.

### Move:

La clase Move contiene una lista de calbacks respecto al juego: dejar carta, coger carta/s, etc.

### Tablero de juego.

Clase que contiene las cartas que hay en la mesa en un momento. Tendrá metodos para meterle cartas y quitarselas.

### GameManager.

Manager general del juego. Controla el resto de managers, los estados del juego, los puntos de los jugadores y demas.

### RoundManager.

Manager encargado de gestionar la partida en si y las rondas, de repartir las cartas y demas. Alomejor es fusionado con el GameManager.

### UIManager.

Manager encargado de gestionar la UI. Inicio del juego, seleccion de partida, partida y resumen de ronda, partida y de juego. Sease una ronda cuando juegan ambas IAs hasta que se acabe el primer mazo, una partida cuando una de las IAs llega a 21 o más puntos y juego cuando se acaben el numero de partidas seleccionadas al principio.


## Implementación:

| Estado | Tarea              |   Fecha    |  Entrega   |
| :----- | :----------------- | :--------: | :--------: |
| Done   | Diseño e idea      | 6-05-2025  | 8-05-2025  |
| Done   | Documentacion      | 14-05-2025 | 15-05-2025 |
| Undone | A: entorno         | 22-05-2025 | 27-05-2025 |
| Undone | B: MCTS            | 25-05-2025 | 27-05-2025 |
| Undone | C: UAI             | 25-05-2025 | 27-05-2025 |
| Undone | D: testing         | 26-05-2025 | 27-05-2025 |
| Undone | E: extra           | xx-xx-2025 | 27-05-2025 |
| Undone | F: extra muy extra | xx-xx-2025 | 27-05-2025 |
| Undone | Pruebas y vídeo    | 26-05-2025 | 27-05-2025 |
| Undone | Build              | 26-05-2025 | 27-05-2025 |

Texto.

## Pruebas y métricas:

Para este proyecto se han realizado las siguientes pruebas:

P1: Dadas unas partidas, que IA ganas más.

P2: Posibles errores al hacer movimientos del MCTS o viceversa.

P3: Posibles errores al hacer movimientos del Utility AI o viceversa.

PX: *A definir durante el desarrollo.*

## Vídeo:

[Aqui va el enlace al video de YouTube](https://github.com/Paigro)

## Build:

[Build](https://drive.google.com/drive/folders/1BzGojaBmIztY5iub1i61UxNumUTViscE?usp=sharing)

## Referencias:

### Assets:

[Cartas](https://fran-ko.itch.io/pixel-deck)

### Asset Store de Unity:

[DOTween](https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676)

### Modelos de IA:

[MCTS1](https://www.geeksforgeeks.org/ml-monte-carlo-tree-search-mcts/)

[MCTS2](https://medium.com/@mattgmez98/using-the-monte-carlo-tree-search-algorithm-for-a-card-game-ai-simulation-40a0218494e4)

[MCTS3](https://ai-boson.github.io/mcts/)

[Utility AI1](https://medium.com/@morganwalkupdev/ai-made-easy-with-utility-ai-fef94cd36161)

[Utility AI2](https://shaggydev.com/2023/04/19/utility-ai)

