# Monopoly
Le but de ce projet est de programmer un Monopoly classique.

## Règles du Monopoly 

### Comment jouer ?

Il suffit de lancer deux dés de 6 pour déplacer son pion du résultat. C'est un jeu de gestion tour-par-tour.
Chaque joueur possède le même montant d'argent, du moins au début. L'objectif est d'être le dernier joueur avec de l'argent sur le plateau.

### Comment on perd ou gagne de l'argent ?

Le plateau de jeu est composé de 40 cases. Chacune de ces cases possède un "effet" :

|Type de case             |Possibilité d'achat ? |Nombre de cases concernées|Effet |
|-------------------------|----------------------|--------------------------|------|
|Propriétés investissable |          Oui         | 22  | Si il appartient à un joueur on est obligé de payer un loyer au propriétaire|
|Gare ou aéroport|Oui|4|Le prix augmente plus on a de propriétés de ce type (avec un maximum de 4 propriétés)|
|Prison|Non|1|Pour s'en échapper il faut payer une certaine somme ou bien avoir une carte "Libéré de prison" ou encore faire un double (3 chances). Le prisonnier ne perçoit aucun revenu le temps de son incarcération.|
|Parc gratuit|Non| 1 |Comme on est tous radin et bien on en profite à fond et on passe un tour|


### Lien pour le drive du projet 

https://drive.google.com/drive/folders/1_7gD0-ItpbNGI8NN9eIbT86-1N43MB6v?usp=sharing
