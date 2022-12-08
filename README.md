# Projet LetsGoBiking

## Terminal Windows

La plupart des étapes suivantes nécessitent l'outil Terminal Windows, si vous n'avez pas installé cet outil, des indications complémentaires seront fournies pour chaque étape afin que vous puissiez tout de même lancer le projet.

## 1 - Lancer ActiveMQ

Dans un premier temps, il faut lancer activeMQ, en ouvrant un cmd dans le dossier activeMQ et en exécutant la commande activemq start.

## 2 - Lancer les serveurs

Pour lancer les serveurs, il suffit d'exécuter le fichier [Servers.bat](Servers.bat) présent à la racine du projet.<br>
⚠️ Il faut vérifier que les executables des serveurs sont bien générés dans le dossier `bin\Debug` de chaque projet, sinon il faut les générer avec Visual Studio.
+ [ProxyServer.exe](LetsGoBikingServer/ProxyService/bin/Debug/ProxyService.exe)
+ [LetsGoBikingServer.exe](LetsGoBikingServer/LetsGoBikingServer/bin/Debug/LetsGoBikingServer.exe)

Si vous ne possédez pas Terminal Windows, vous pouvez lancer les serveurs manuellement en exécutant les deux fichiers `.exe` ci-dessus (dans l'ordre).

## 3 - Lancer le client

Pour lancer le client, il suffit d'exécuter le fichier [Client.bat](Client.bat) présent à la racine du projet.<br>

Si vous ne possédez pas Terminal Windows, vous pouvez lancer le client manuellement en exécutant la commande suivante dans le dossier `client` :
```mvn compile exec:java```