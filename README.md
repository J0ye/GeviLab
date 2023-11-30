# GeviLab

## Schritte zur Anpassung der UI
### Es gibt In-World UI und Screen-Size UI
### In-World UI
- Als In-World UI zählt jede Form von Annotationen (Assets > Prefabs > Annotation), also:
  - Notes, Images, Video, Gallery
  - Diese Elemente sollten nur über das Prefab geändert werden
  - Permanent Annotations (Assets > Prefabs > Annotation > Permanent*.prefab), sind Annotationen die im Editor gesetzt werden und nicht löschbar sind (permanent).
- Als In-World UI zählen auch die Pfeile, die zum Wechseln der Umgebung verwendet werden
  - Wegen speziellen Verlinkungen gibt es hier für kein Prefab.
  - Stattdessen hat jedes Environment (Entry, ControllroomRight, Turbine, etc) eigene Instanzen.
  - Die verlinkten Umgebungen werden über die Environment Bridge Komponente angeben (Komponente an jedem Envrionment)
### Screen-Size UI
- Als Screen-Size UI zählt das Optionsmenü, die Log-Texte und das Quiz.
- Alle Elemente in Scene Root > UI sind auch in VR (X Taste auf dem linken Controller zum öffnen) auswählbar
- Das Quiz muss über das Prefab angepasst werden (Assets > Prefabs > Questionnaire > TQV Test Questionnaire Variant.prefab)
  - Jedes Quiz (im Moment gibt es nur eins) wird aus Questions (Assets > Prefabs > Questionnaire > Question.prefab) gebaut, welche wiederum aus SelectionOptions (Assets > Prefabs > Questionnaire > SelectionOption.prefab) gebaut werden
  - **Quiz** -> Mit Manager (NetworkQuestionaireController) für speichern und senden
    - **Question** -> Mit Manager (QuestionController) zum setzten und verwalten von Inhalten (Wird nicht automatisch erstellt, sondern muss im Editor gesetzt werden)
      - **SelectionOption** -> Werden automnatisch durch (QuestionController) erstellt. Reagieren auf klicken durch Nutzer
    - Buttons -> Zum Wechseln der angezeigten Frage und zum Schließen der Anzeige
- Das Settings Menü sollte über das Prefab (Assets > Prefabs > Menus) angepasst werden
  - Die Verlinkungen sind jedoch szenenspezifisch
- Ausgaben für den Log Creator können aus jedem Skript über LogCreator.instance.AddLog(""); erstellt werden.  
