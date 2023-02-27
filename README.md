# LeagueProfiles

This applicaton served to me as a practice ground for all I believe there is to the basics of .NET backend development. It realises a code-first EF many-to-many relationship between users (player) and their bi-directionally owned object (champion). The application as it stands now is tiny, however just for practice sake async functionality was implemented when making database calls here and there. For the authentication and authorisation aspect JWT is used, despite the obvious risks of using tokens and letting them exist for a long time.

TODO:
- Exception handler
- Refactor and move some functionality to their respective classes
- Possibly begin an Angular project to provide a frontend application (In progress)
