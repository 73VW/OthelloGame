OthelloGame
===========

Fonctionnalités intéressantes
-----------------------------

Style Space Invaders
^^^^^^^^^^^^^^^^^^^^

Dans l'idée de réaliser un design minimaliste mais efficace, nous avons décidé de créer un style rétro.
Space Invaders étant un classique du genre, c'est tout naturellement que notre choix s'est porté sur cette idée.

Indication du tour
^^^^^^^^^^^^^^^^^^

Les joueurs doivent poser les pions tour à tour.
Afin de reconnaître qui doit jouer, l'icône du joueur actif devient opaque tandis que celle du joueur adverse devient semi-transparente.

Indication du meneur
^^^^^^^^^^^^^^^^^^^^

Le fond d'écran du jeu est variable. Si les deux joueurs ont un score équivalent, le fond est noir.
Par contre, si l'un des deux joueurs prend l'avantage, son icône devient l'image de fond de la grille de jeu.

Animations
^^^^^^^^^^

La technologie utilisée ne permettant pas de faire des animations entre images de fond, des recherches ont été nécessaires afin de trouver une solution.
Grace à un utilisateur de Stackoverflow (lien dans le fichier correspondant), il a été possible de réaliser ces animations.

De ce fait, lorsqu'un pion est capturé, le fond de la case ne change pas brutalement mais via une transition qui dure deux dixièmes de seconde.

De plus, l'indication du meneur de jeu subit aussi les animations.

Undo/Redo
^^^^^^^^^

Afin de pouvoir annuler un coup ou le rejouer, le Design Pattern "Command" a été implémenté. Il permet de contrôler les actions effectuées par les jouers et revenir en arrière ou en avant au besoin.

Sauvegarde de la partie
^^^^^^^^^^^^^^^^^^^^^^^

Grâce à la technologie employée (c#), il est facile d'implémenter un système de sauvegarde de partie. De ce fait, il est possible pour les joueur de sauvegarder leur partie et de la recharger au besoin.

Reset
^^^^^

Il est possible de recommencer le jeu à zéro en appuyant sur le bouton "Reset".

Nom du joueur
^^^^^^^^^^^^^

Au lancement du jeu, un formulaire demande le nom des joueurs. Il est possible de garder les noms par défaut, soit "Player 1" et "Player 2".
