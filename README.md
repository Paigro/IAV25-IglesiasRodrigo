# IAV25-IglesiasRodrigo

## Info:

Proyecto final de la asignatura de Inteligencia Artificial de Videojuegos del GdV UCM.

[Pablo Iglesias Rodrigo](https://github.com/Paigro)

## Resumen:

Partida de La Escoba entre 2 IAs con modelos diferentes modelos de IA.

## Idea:

Juego de cartas de la baraja española de La Escoba en Unity. La idea es hacer 2 IAs que jueguen entre ellas con la posibilidad de poder tomar el control de una de ellas por el jugador si asi lo desea. Se implementaran 2 modelos de IA: Monte Carlo Tree Search y Utility AI, y antes de empezar la partida se podrá determinar cual de los dos tendrá cada IA. Luego jugarán.

## Planteamiento del problema:

A - Creacion del entorno de juego. Creación del tablero, el mazo y manos. El GameManager, UIManager y LevelManager. Los assets serán sacados de Internet si no da tiempo a crearlos.
 
B - Crear el MCTS.

C - Crear el UtilityAI.

D - Testing de partida. Comporbar que el juego se juega, las rondas y partidas van como deberían y no hay errores.

E - Usuario jugador. Implementar que el jugador pueda tomar el control de una de las IAs (desde el menu entre rondas), tapando las cartas de la otra IA. Tambien se podra devolver el control a la IA.

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

    function untriedActions() -> list:
      untriedActions = state.getPossibleActions()
      return untriedActions

    function getNVisits() -> int:
      return nVisits

    function expands() -> MCTSNode:
      action = untriedActions.pop() # Pick the first action to make.
      nextState = state.move(action) # Simulates the next state base in the action taken.
      newChild = MCTSNode(nextState, this, action) # Creates a new child with the new properties.
      
      children.add(newChil) # Adds the child to the list of childs.
      return newChild

    function isEnough() -> bool:
      return accPoints >= 15

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
        parent.backpropagate()

    function isFullyExpanded() -> bool:
      return (untriedActions.size == 0)

    function select(node) -> MCTSNode:
      while node.isFullyExpanded() and node.children.count == 0:
        node = node.bestChild(c = √2)

    function bestChild(node, c) -> MCTSNode:
      return argmax_{child in node.children} (hijo.accPoints / hijo.nVisits + C * sqrt(ln(node.nVisits) / hijo.nVisits)
  )
      
  
  
```

```
class UtilityAI:
  
```

## Clases extras:

Carta.

Mazo. Barajado.

Mano.

Tablero de juego.

GameManager.

Rondas y partidas.

Level Manager.

UI manager.


## Implementación:

| Estado  |  Tarea  |  Fecha  |  Entrega  |  
|:--|:--|:-:|:-:|
| Done | Diseño e idea | 6-05-2025-2025 | 8-05-2025 |
| In progress | Documentacion | 14-05-2025-2025 | 14-05-2025 |
| Undone | A: entorno | xx-xx-2025 | 27-05-2025 |
| Undone | B: MCTS | xx-xx-2025 | 27-05-2025 |
| Undone | C: UAI | xx-xx-2025 | 27-05-2025 |
| Undone | D: testing | xx-xx-2025 | 27-05-2025 |
| Undone | E: extra | xx-xx-2025 | 27-05-2025 |
| Undone | Pruebas y video | xx-xx-2025 | 27-05-2025 |
| Undone | Build | xx-xx-2025 | 27-05-2025 |

Texto.

## Pruebas y métricas:

Para este proyecto se han realizado las siguientes pruebas:

P1:

P2:

P3:

## Vídeo:

Texto.

## Build:

Texto.

## Notas:

A - base; creada por nosotros, cogida de las practicas o externo
B y C - cosas de as prácticas pero no obligatorio.
D y E - cosas nuevas añadidas. Puede ser solo 1, etc.

## Referencias:

[Monte Carlo Tree Search](https://www.geeksforgeeks.org/ml-monte-carlo-tree-search-mcts/)

[MCTS](https://medium.com/@mattgmez98/using-the-monte-carlo-tree-search-algorithm-for-a-card-game-ai-simulation-40a0218494e4)

[Utility AI1](https://medium.com/@morganwalkupdev/ai-made-easy-with-utility-ai-fef94cd36161)

[Utility AI2](https://shaggydev.com/2023/04/19/utility-ai)
