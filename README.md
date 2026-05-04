# CyberScout

&nbsp;

CyberScout is a collection of software designed to enhance the [FIRST Robotics Competition](https://www.firstinspires.org/robotics/frc) scouting and strategizing experience. CyberScout is primarily developed and maintained by Andrew Willms for [FRC team 4678, the CyberCavs](https://www.cybercavs.com/), but is designed to be highly customizable such that many teams may find it useful.

&nbsp;

The goal of CyberScout is to
- eliminate paper scouting and manual data entry,
- enable robust, multi-modal data synchronization so teams don't need to rely on an internet connection, and 
- enable teams to easily create custom data collection interfaces and schemas without needing to code.

&nbsp;

The main components of CyberScout are listed below:

- GameMaker: A (currently Windows-only) app enabling the specification of custom data collection interfaces and schemas.

- Server: An (under construction) web server to provide web based data syncronization when connectivity is available.

- ScoutingApp: A (currently Android-only) app that displays the scouting interface based on the game specifications and generates QR codes containing the scouted data.

- DataViewer: A (concept for a) cross-platform interface for querying, displaying, and comparing the collected data. (Currently the scouting app exports a CSV).

&nbsp;

CyberScout was first conceptualized in late 2019 and has been under (sporadic) development since. The CyberCavs have used some form of CyberScout in 2022, 2023, 2025, and 2026 with each year bring new features and improvements. Optimistically, CyberScout is intended to have a more public-read release for the 2027 FRC season.
