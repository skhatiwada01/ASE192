# OSS Bug Localization Dataset

The dataset contains bug localization data from three open source projects:
1. Angular.js (Project Id: 460078)
2. Corecrl (Project Id: 30092893)
3. Kubernetes (Project Id: 20580498)

### Dataset Structure
Each dataset consists of the following folder and files:

#### Issues
This folder contains files that holds issues from the project in their original form. Each file contains multiple issue stored in xml format. Each issue contains its Id, IssueNumber, AssigneeId, CreatedBy UserId, Title, and Body.

#### Localization
This folder contains multiple folders, each representing an issue used for bug localization. Each folder contains 4 files:

##### BugReport.txt
This file contains the bug report text stemmed and camel-case splitted. 

##### FileList.txt
This file contains the list of all files in the system. For each file, the relative path of the file and an arbitary unique id, index, is provided. The file is in xml format.

##### RelevantList.txt
This file contains the list of all **relevant** file(s) for the issue. For each file, the relative path of the file and the index is provided. The file is in xml format.

##### Source.txt
This file contains the source code for the entire system. The source code belongs to the version of the project where the fix was applied. Each source code file is represented as a single line in Source.txt. The text are stemmed and camel-case splitted. The file is in following format: FileIndex<sub>1</sub>##word<sub>11</sub>,word<sub>12</sub>,word<sub>13</sub>,...,word<sub>1n</sub> FileIndex<sub>2</sub>##word<sub>21</sub>,word<sub>22</sub>,word<sub>23</sub>,...,word<sub>2m</sub>

#### AllCommits.txt
This file contains all commits made in the project. Each commit info contains the commit's Sha, data and time the commit was made, and the UserId of the committer. The file is in xml format.

#### CommitChanges.txt
This file contains the list of source code files modified in each commit. It contains the source file's relative file path and it's status. The file is in xml format.

#### IssueCommits.txt
This file contains the issue numbers and the commit's sha associated with that issue. The file is in xml format.

#### Tags.txt
This file contains every tag name, color, and the url. Each tag is assigned an arbitary, unique id. The file is in xml format.

#### IssueTags.txt
The file contains the the issue numbers and the tags associated with each issue. The file is in the follwoing format:
IssueNumber<sub>1</sub>##Tag<sub>11</sub>,Tag<sub>12</sub>,Tag<sub>13</sub>,...,Tag<sub>1n</sub>
IssueNumber<sub>2</sub>##Tag<sub>21</sub>,Tag<sub>22</sub>,Tag<sub>23</sub>,...,Tag<sub>2m</sub>

### Dataset Creator
The downloader.exe download all files required for creating bug localization dataset. The downloader.exe contains the following main sections:

#### Github Login
This section logs user to Github account. A Github account is required to download project information off Github.

#### Repository Info
This section sets the github repository and the directory where the repository is to be downloaded. 

#### Action
This section provides different functionality to download tags, bug reports, source codes, commits, and to create the dataset. The **Bug Localization** button opens up a form, Localization Form, to run IR methods for bug localization.

### Contents

The downloader.exe is in the exe folder.
The code implementation is in the src folder.
The dataset for the projects can be downloaded from: http://seel.cse.lsu.edu/data/ase192.zip
