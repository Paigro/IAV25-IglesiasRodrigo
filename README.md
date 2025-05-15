# IAV25-IglesiasRodrigo

## Info:

Proyecto final de la asignatura de Inteligencia Artificial de Videojuegos del GdV UCM.

[Pablo Iglesias Rodrigo](https://github.com/Paigro)

## Resumen:

Partida de La Escoba entre 2 IAs con modelos diferentes modelos de IA.

## Idea:

Juego de cartas de la baraja española de *La Escoba* en Unity. La idea es hacer 2 IAs que jueguen entre ellas con la posibilidad de poder tomar el control de una de ellas por el jugador si asi lo desea. Se implementaran 2 modelos de IA: *Monte Carlo Tree Search* y *Utility AI*, y antes de empezar la partida se podrá determinar cual de los dos tendrá cada IA. Luego jugarán n partidas 

## Planteamiento del problema:

**A** - Creacion del entorno de juego. Creación del tablero, el mazo y manos. El GameManager, UIManager y LevelManager. Los assets serán sacados de Internet si no da tiempo a crearlos.
 
**B** - Crear el modelo de IA MCTS. Le modelo va recorriendo un arbol de jugadas dadas las posibles cartas que tiene y hay en la mesa para escoger la mejor jugada.

**C** - Crear el modelo de IA UtilityAI. El modelo asigna pesos a las cartas o jugadas (escoba, 7 de oros, sietes, oros y cartas) y dadas todas las combinaciones, hace la heuristica y se queda con el mejor resultado.

**D** - Testing de partida. Comporbar que el juego se juega, las rondas y partidas van como deberían y no hay errores.

**E** - Usuario jugador. Implementar que el jugador pueda tomar el control de una de las IAs (desde el menu entre rondas), tapando las cartas de la otra IA. Tambien se podra devolver el control a la IA.

## Diseño de la solución:

```
class MCTSNode(state, parent = none, parent_action = none):
  state = state # Represents the state of the game. Cards in hand, cards on table and maybe there is chance to memorice the cards that has been taken by itself and/or oponent.
  parentNode = parent # Reference to previous node.
  parentAction = parent_action # Reference the action taken from the parent node. 
  children: list # Possible actions from this node.
  nVisits: int # Number of visits this node has been visited
  accPoints: int # Number of points accumulated in this node.
  untriedActions: list = state.getPossibleActions() # Actions that are lef to try.

    function expands() -> MCTSNode:
      action = untriedActions.pop() # Pick the first action to make.
      simState = state.move(action) # Simulates the next state base in the action taken.
      newChild = MCTSNode(simState, this, action) # Creates a new child with the new properties.
      children.add(newChild) # Adds the child to the list of childs.
      return newChild

    function rollout() -> float:
      simState = state # Copy of the state to simulate.
      while not simState.isEnough():
        possibleMoves = simState.getPossibleActions()
        action = rolloutPolicy(possibleMoves)
        simState = simState.move(action)
      return simState.evaluate()

    function backpropagate(result) -> void
      if this == null:
        return
      nVisits += 1
      accPoints += result
      if parent:
        parent.backpropagate(accPoints)

    function isFullyExpanded() -> bool:
      return (untriedActions.size == 0)

    function select() -> MCTSNode:
      node = this
      while !node.isFullyExpanded() and node.children.count > 0:
        node = node.bestChild(c = √2)

    function bestChild(c) -> MCTSNode:
      return argmax_{child in this.children} ((child.accPoints / child.nVisits) + c * sqrt(ln(this.nVisits) / child.nVisits)) # With UCB1.

  
function isEnough() -> bool:
  return accPoints >= 15

function MCTS(initState, maxTime) -> Action:
  initNode = MCTSNode(initState)
  initTime = getTime()
  while (getTime() - initTime) < maxTime:
    node = select()
    if not node.isFullyExpanded():
      node = expands()
    result = rollout()
    node.backpropagate(result)
  bestChild = argmax_{h in rootNode.children} h.nVisits
  return bestChild.parentAction

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
    sevensInMove = 0 # 0, 1 or 2 deppens on the move.
    goldsInMove = 0 # 0, 1, ... deppens on the move.
    cardsInMove = 0 # 0, 1,... deppends on the move.

    if move.hasScopa():
      scopaInMove = 1
    
    if move.hasGoldenSeven()
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
             cards[j + i * 10] = new Card('O', j)
            case 1:
             cards[j + i * 10] = new Card('E', j)
            case 2:
             cards[j + i * 10] = new Card('C', j)
            case 3:
             cards[j + i * 10] = new Card('B', j)
    nCards = 40

  # Shuffle the cards randmoly
  fucntion shuffle(nTimes = 1) -> void:
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

Clase que contiene las cartas que ha ido recogiendo cada jugador en una ronda para luego hacer la cuenta final y contabilizar puntos. Para mayor facilidad llevara tambien la cuenta de escobas.

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

| Estado      | Tarea           |   Fecha    |  Entrega   |
| :---------- | :-------------- | :--------: | :--------: |
| Done        | Diseño e idea   | 6-05-2025  | 8-05-2025  |
| In progress | Documentacion   | 14-05-2025 | 14-05-2025 |
| Undone      | A: entorno      | xx-xx-2025 | 27-05-2025 |
| Undone      | B: MCTS         | xx-xx-2025 | 27-05-2025 |
| Undone      | C: UAI          | xx-xx-2025 | 27-05-2025 |
| Undone      | D: testing      | xx-xx-2025 | 27-05-2025 |
| Undone      | E: extra        | xx-xx-2025 | 27-05-2025 |
| Undone      | Pruebas y video | xx-xx-2025 | 27-05-2025 |
| Undone      | Build           | xx-xx-2025 | 27-05-2025 |

Texto.

## Pruebas y métricas:

Para este proyecto se han realizado las siguientes pruebas:

P1: Dadas unas partidas, que IA ganas más.

P2: Posibles errores al hacer

P3:

## Vídeo:

Texto.

## Build:

Texto.

## Notas:

A - base; creada por nosotros, cogida de las practicas o externo
B y C - cosas de las prácticas pero no obligatorio.
D y E - cosas nuevas añadidas. Puede ser solo 1, etc.

## Referencias:

[MCTS1](https://www.geeksforgeeks.org/ml-monte-carlo-tree-search-mcts/)

[MCTS2](https://medium.com/@mattgmez98/using-the-monte-carlo-tree-search-algorithm-for-a-card-game-ai-simulation-40a0218494e4)

[MCTS3](https://ai-boson.github.io/mcts/)

[Utility AI1](https://medium.com/@morganwalkupdev/ai-made-easy-with-utility-ai-fef94cd36161)

[Utility AI2](https://shaggydev.com/2023/04/19/utility-ai)