# StockMaster

Auswertungsprogramm für den Stocksport
- Erzeugen von Wertungskarten (auch mit 8 Kehren)
- Anzeige von Live-Spielständen in Verbindung mit StockTV
- Übernahme der Werte von StockTV in die Ergebnisse
- Drucken von Quittungen
- Erzeugen von Ergebnislisten
- Erzeugen von Bahnblöcken (auch mit 8 Kehren)
- Speichern und Laden von Turnieren

**NEU: Integration von Zielwettbewerben**


### StockTV Netzwerkverbindung
StockMaster kann mit StockTV kommunizieren. Genauer gesagt, es kann Daten von StockTV empfangen.
Was ist dabei zu beachten:
- Auf dem Rechner auf dem StockMaster ausgeführt wird, darf die Firewall den Netzwerkverkehr nicht blockieren.
- Der Rechner mit StockMaster muss sich im gleichen Netzwerk (auch Subnetz) befinden
- Bei WLAN kann es in Verbindung mit Repeater Probleme geben. Alle Geräte sollten den gleichen AccessPoint (Repeater) nutzen
- Die Firewall unterscheidet verschiedene Netzwerkbereiche. Privat, Öffentlich (und Domain).
- Die Firewall-Regel muss mindestens für den Netzwerkbereich freigegegebn sein, in dem sich der Rechner zum Zeitpunkt der Auswertung befindet
- StockMaster lauscht auf Port 4711 UDP

#### Empfehlung
Es empfiehlt sich, alle bestehende Regeln bzgl. der StockMaster.exe zu löschen und eine universielle Regel zu erstellen.
- Windows Firewall Editor starten: wf.msc
- auf der linken Seite die "eingehende Regeln" wählen
- in der mittleren Ansicht am Besten nach Name sortieren und alle Einträge mit stockmaster.exe löschen
- in der Eingabeaufforderung (Dos-FensteR) folgenden Befehl ausführen.
  C:\>netsh firewall set portopening UDP 4711 “StockMaster4711” enable all
- Im Fenster mit den Firewall-Regeln die Ansicht aktualisieren.
- Die neu erstellte Regel suchen und doppelklicken
- Im Register "Erweitert" alle drei zur verfügung stehenden Profile aktivieren
- mit OK bestätigen
