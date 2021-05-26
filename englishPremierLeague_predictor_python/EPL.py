import pandas as pd
import seaborn as sns
import numpy as np
from sklearn import metrics
import seaborn as sns
import matplotlib.pyplot as plt
from sklearn.ensemble import RandomForestClassifier
from sklearn.model_selection import train_test_split
from sklearn.linear_model import LogisticRegression

from sklearn.feature_selection import SelectKBest
from sklearn.feature_selection import chi2
from sklearn.ensemble import ExtraTreesClassifier
from sklearn.linear_model import LinearRegression, Ridge
from sklearn.ensemble import GradientBoostingRegressor
from sklearn.metrics import mean_squared_error
from sklearn.model_selection import RandomizedSearchCV
from sklearn.svm import SVC
from sklearn.multiclass import OneVsRestClassifier

import copy

TEAMS = ["Arsenal", "Bournemouth", "Brighton", "Burnley", "Cardiff",
         "Chelsea", "Crystal Palace", "Everton", "Fulham", "Huddersfield",
         "Leicester", "Liverpool", "Man City", "Man United", "Newcastle",
         "Southampton", "Tottenham", "Watford", "West Ham", "Wolves"]

# These features are the ONLY important features to consider
FEATURES = ['HomeTeam', 'AwayTeam', 'FTHG', 'FTAG', 'FTR', 'HS', 'AS', 'HST', 'AST', 'HF', 'AF', 'HC', 'AC', 'HY', 'AY',
            'HR', 'AR']

'''
These features are features that don't tell us anything about the outcome of a match. The reason we separate them from the other features
is because we want to ONLY look at the features that don't predict the outcomes/scores of a match. If we included features that could
help us determine which team won a match (ex: "FTHG"), then we would already know the outcome of a certain match, and so using ML in this
situation would be pointless
'''

predictive_FEATURES = ['HomeTeam', 'AwayTeam', 'FTHG', 'FTAG', 'FTR']
NonPredictive_FEATURES = ['HS', 'AS', 'HST', 'AST', 'HF', 'AF', 'HC', 'AC', 'HY', 'AY', 'HR', 'AR']


# This creates an additional column (within our master dataframe) that represents the outcomes of each match.
# If home team won, it returns 1; if match ended in a draw, it returns 0; if home team lost, it returns -1.
def matchOutcomes(result):
    if result[0] == 'H':
        return 1
    elif result[0] == 'D':
        return 0
    else:
        return -1


# Cleans, and returns the data
def Clean_and_Get_Data():
    '''
    data1 = pd.read_csv('data/2001.csv')
    data2 = pd.read_csv('data/2002.csv')
    data3 = pd.read_csv('data/2003.csv')
    #data4 = pd.read_csv('data/2004.csv')
    #data5 = pd.read_csv('data/2005.csv')
    data6 = pd.read_csv('data/2006.csv')
    data7 = pd.read_csv('data/2007.csv')
    data8 = pd.read_csv('data/2008.csv')
    data9 = pd.read_csv('data/2009.csv')

    data10 = pd.read_csv('data/2010.csv')
    data11 = pd.read_csv('data/2011.csv')
    '''
    data12 = pd.read_csv('data/2012.csv')
    data13 = pd.read_csv('data/2013.csv')
    data14 = pd.read_csv('data/2014.csv')
    data15 = pd.read_csv('data/2015.csv')
    data16 = pd.read_csv('data/2016.csv')
    data17 = pd.read_csv('data/2017.csv')
    data18 = pd.read_csv('data/2018.csv')
    '''
    data1 = data1.loc[:, FEATURES]
    data2 = data2.loc[:, FEATURES]
    data3 = data3.loc[:, FEATURES]
    #data4 = data4.loc[:, FEATURES]
    #data5 = data5.loc[:, FEATURES]
    data6 = data6.loc[:, FEATURES]
    data7 = data7.loc[:, FEATURES]
    data8 = data8.loc[:, FEATURES]
    data9 = data9.loc[:, FEATURES]
    data10 = data10.loc[:, FEATURES]
    data11 = data11.loc[:, FEATURES]
    '''
    data12 = data12.loc[:, FEATURES]
    data13 = data13.loc[:, FEATURES]
    data14 = data14.loc[:, FEATURES]
    data15 = data15.loc[:, FEATURES]
    data16 = data16.loc[:, FEATURES]
    data17 = data17.loc[:, FEATURES]
    data18 = data18.loc[:, FEATURES]

    # dataFrames = [data1, data2, data3, data6, data7, data8, data9, data10, data11, data12, data13,  data14, data15, data16, data17, data18]
    # dataFrames = [data10, data11, data12, data13,  data14, data15, data16, data17, data18]
    dataFrames = [data12, data13, data14, data15, data16, data17, data18]

    masterData = pd.concat(dataFrames)

    # This adds a new column (titled "Outcome") to our data. While looking at all of the matches played, if the home team won,
    # we "give" that match a value of "1"; if the home team lost, we "give" that match a value of "-1"; otherwise we give the
    # match a value of "0". Please refer to the "matchOutcomes" function.
    masterData['Outcome'] = masterData[['FTR']].apply(matchOutcomes, axis=1)
    # print(masterData)

    return masterData


# Returns the matches from the previous (2018) season
def GetPreviousSeasonData():
    data = pd.read_csv('data/2018.csv')
    data = data.loc[:, FEATURES]

    data['Outcome'] = data[['FTR']].apply(matchOutcomes, axis=1)
    return data


# Returns the matches from the current (2019) season
def GetCurrentSeasonData():
    data = pd.read_csv('data/2019.csv')
    data = data.loc[:, FEATURES]

    data['Outcome'] = data[['FTR']].apply(matchOutcomes, axis=1)
    return data


# Chi-Squared (univariate selection) - applying SelectKBest to get top 10 features
def ChiSquared(knum, x, y):
    bestfeatures = SelectKBest(score_func=chi2, k=knum)
    fit = bestfeatures.fit(np.nan_to_num(x), y)

    dfscores = pd.DataFrame(fit.scores_)
    dfcolumns = pd.DataFrame(NonPredictive_FEATURES)

    featureScores = pd.concat([dfcolumns, dfscores], axis=1)  # concatiante 2 data frames - better visualization
    featureScores.columns = ['Stat', 'Score']  # name data frame columns

    # print(featureScores)
    print(featureScores.nlargest(knum, 'Score'))  # print top 10 features


# Feature Importance - applying feature_importance tree-based classifiers to get top 10 features
def FeatureImportance(knum, x, y):
    # Feature Importance - applying feature_importance tree-based classifiers to get top 10 features
    model = ExtraTreesClassifier()
    model.fit(np.nan_to_num(x), y)

    # use built in class
    print(model.feature_importances_)

    # plot graph of feature importance
    dfcolumns = pd.DataFrame(NonPredictive_FEATURES)
    feat_importances = pd.Series(model.feature_importances_, index=dfcolumns)
    feat_importances.nlargest(knum).plot(kind='barh')
    plt.show()


'''
This function uses logistic regression to predict the outcome of a match between a home team and away team.
The training data consists of the master data and the testing data consists of the 2019 season statistics which can either 
be Zul's generated statistics or the actual 2019 season statistics. 
Additionally, we output the predicted standings of the 2019 season. 
Accuracy is recorded by taking the number of correctly predicted outcomes and dividing it by the total match outcomes.
'''
def LogReg(masterData, currentSeason, s):
    # extract the label and training data
    y = masterData['Outcome'].values
    x = masterData.loc[:, NonPredictive_FEATURES]
    x = x.loc[:, x.columns.intersection(['HST', 'AST', 'AS', 'HS', 'HR', 'HY'])]
    x = x.values

    # store the home and away team
    home_teams = GetCurrentSeasonData().loc[:, "HomeTeam"]
    away_teams = GetCurrentSeasonData().loc[:, "AwayTeam"]

    # create a dictionary of teams to store their points
    standing_predictions = {}
    for team in home_teams:
        standing_predictions[team] = 0

    # Predicted 2019 season statistics
    if s == 0:
        currentFeatures = currentSeason.loc[:, NonPredictive_FEATURES]
        currentFeatures = currentFeatures.loc[:, currentFeatures.columns.intersection(['HST', 'AST', 'AS', 'HS', 'HR', 'HY'])]
        currentFeatures = currentFeatures.values
    else:
        currentFeatures = currentSeason
    # Actual 2019 outcomes
    y_actual = GetCurrentSeasonData().loc[:, "Outcome"]

    # build and train the model using the master data
    model = LogisticRegression(max_iter=2000, solver='lbfgs')
    model.fit(np.nan_to_num(x), y)
    # test the model on the testing data
    y_predicted = model.predict(currentFeatures)

    for outcome, home, away in zip(y_predicted, home_teams, away_teams):
        if outcome == 1:
            standing_predictions[home] += 3
        elif outcome == -1:
            standing_predictions[away] += 3
        else:
            standing_predictions[home] += 1
            standing_predictions[away] += 1

    count_misclassified = (y_actual != y_predicted).sum()
    #print('Misclassified samples: {}'.format(count_misclassified))
    accuracy = metrics.accuracy_score(y_actual, y_predicted)
    #print('Accuracy: {:.2f}'.format(accuracy))

    finalStandings = sorted(standing_predictions.items(), reverse=True, key=lambda x: x[1])

    # printing the results of the season
    print("Using Logistic Regression the season results are: ")
    for team in finalStandings:
        print(team[0], " ", team[1])
    print("The accuracy is: " + str(accuracy * 100))


#this algorithm uses svm to simulate a match, what you do is pass in masterdata you want to train the model and sim data you want to simulate the season
def SVM(masterData, simdata, simdata2019, cases):
    hometeams = simdata.loc[:, "HomeTeam"]
    awayteams = simdata.loc[:, "AwayTeam"]

    # creates a dictionary
    simu = {}

    sim_data = []

    for t in hometeams:
        simu[t] = 0

    y = masterData['Outcome'].values
    np_Data = masterData.loc[:, NonPredictive_FEATURES]
    np_Data = np_Data.loc[:, np_Data.columns.intersection(['HST', 'AST', 'AS', 'HS', 'HR', 'HY'])]
    simy = simdata['Outcome'].values

    if cases == 0:
        sim_data = simdata.loc[:, NonPredictive_FEATURES]
        # only keeping important feature with chi square

        sim_data = sim_data.loc[:, sim_data.columns.intersection(['HST', 'AST', 'AS', 'HS', 'HR', 'HY'])]
    else:
        sim_data = simdata2019

    x = np_Data.values

    xsim_data = sim_data.values

    x = np.nan_to_num(x)
    xsim_data = np.nan_to_num(xsim_data)

    # trains the model if you want to use binary classification uncomment this if you want that and comment oneVsrestclassifier
    # clf = SVC()
    # clf.fit(x, y)

    clf = OneVsRestClassifier(SVC(gamma='auto')).fit(x, y)

    predictedsimu = clf.predict(xsim_data)


    #print("the accuracy is ", clf.score(xsim_data,))
    # calculating the points in the season
    for t, x, z in zip(predictedsimu, hometeams, awayteams):
        if t == 1:
            simu[x] += 3
        elif t == -1:
            simu[z] += 3
        else:
            simu[x] += 1
            simu[z] += 1

    # sorting it
    listofTuples = sorted(simu.items(), reverse=True, key=lambda x: x[1])

    # printing the results of the season
    print("Using SVM the season results are: ")
    for elem in listofTuples:
        print(elem[0], " ", elem[1])

    print("The accuracy is ", clf.score(xsim_data, simy) * 100)



def VerifyRegressors(individualTeamsStats_dataFrame, individualTeamStats):
    regressionModels = [LinearRegression(), Ridge(), GradientBoostingRegressor()]
    x = individualTeamsStats_dataFrame.values
    y = individualTeamsStats_dataFrame.iloc[:, 1]  # "Goals scored by each team

    print(x)
    print(y)

    # Root Mean Square Errors
    RMSEs = []
    CVs = []

    for i in regressionModels:
        RMSEs.append(Calculate_Model_RMSE(x, y, i))

    print(RMSEs)

    CVs.append(RegressionModel_CV(x, y, Ridge()))


def Calculate_Model_RMSE(x, y, model):
    x_train, x_test, y_train, y_test = train_test_split(x, y, test_size=0.2)

    model.fit(x_train, y_train)
    modelPredictions = model.predict(x_test)

    RMSE = mean_squared_error(y_test, modelPredictions) ** .5
    return RMSE


def RegressionModel_CV(x, y, model):
    ridge_params = {'alpha': [10 ** -i for i in range(0, 8)]}
    ridge_cv = RandomizedSearchCV(model, ridge_params, cv=5, n_jobs=-1)

    z = ridge_cv.fit(x, y)
    print(z)


# Creates a model that predicts the number of goals scored by each team in a match
def ExpectedGoals_Model(matchStats_df, goalsScored, classifier):
    x = matchStats_df.values
    y = goalsScored  # Goals scored by each team

    classifier.fit(np.nan_to_num(x), np.ravel(np.nan_to_num(y)))  # np.ravel = converts 'y' to a 1d array
    return classifier


# Creates a model that predicts the stats (ex: shots, corners, etc) of each team in a match
def Stats_Model(matchStats_df, classifier, feature):
    # This variable represents the 'x' last games of a team we look at
    NUMBER_OF_GAMES = 10

    x_list = []
    y_list = []

    for team in TEAMS:
        team_df = matchStats_df[matchStats_df[0].astype(str).str.contains(team)]
        clean_df = team_df.drop([0, 1, feature],
                                axis=1)  # removes team name, number of goals scored, & stat/feature we want to predict

        for i in range(team_df.shape[0] - NUMBER_OF_GAMES - 1):
            x_vector = clean_df.iloc[i:i + NUMBER_OF_GAMES].values.flatten()

            if x_vector.shape[0] == 6 * NUMBER_OF_GAMES:
                x_list.append(x_vector)
                y_list.append(team_df[feature].values[
                                  i + NUMBER_OF_GAMES + 1])  # this represents the predictions for a specific stat for a certain game

    x = np.vstack(x_list)
    y = np.array(y_list)
    classifier.fit(x, y)

    return classifier


# Predicts the stats of a team for a given match
def Simulate_Game(game, df, clf_map, goal_model, noise_map):
    # This variable represents the 'x' last games of a team we look at
    NUMBER_OF_GAMES = 10

    team_df = df[df[0].astype(str).str.contains(game[0])]

    statsDict = {2: None, 3: None, 4: None, 5: None, 6: None, 7: None}

    for feature in clf_map.keys():
        # Gets rid of team names, goals scored, and home/away column
        x = team_df.tail(NUMBER_OF_GAMES).drop([0, 1, feature], axis=1).values.flatten().reshape(1, -1)
        noise = noise_map[feature] * np.random.normal()
        statsDict[feature] = max(int(clf_map[feature].predict(np.nan_to_num(x))[0]) + noise, 0)

    # We want to use our stats (ex: shots, fouls) to predict the number of goals scored
    featureList = [2, 3, 4, 5, 6, 7]
    statsList = [statsDict[feature] for feature in featureList]

    goals = np.array(statsList).reshape(1, -1)

    statsDict['Score'] = int(goal_model.predict(goals)[0])  # predicted number of goals for the team

    return statsDict


def Calculate_Team_Points(Season2019_predictions_df, teams):
    teamPoints = {team: 0 for team in TEAMS}

    i = 0
    while i < Season2019_predictions_df.shape[0]:
        homeGoals = Season2019_predictions_df.iloc[i, 6]
        awayGoals = Season2019_predictions_df.iloc[i + 1, 6]

        if homeGoals > awayGoals:
            teamPoints[teams[i]] += 3
        elif homeGoals < awayGoals:
            teamPoints[teams[i + 1]] += 3
        else:
            teamPoints[teams[i]] += 1
            teamPoints[teams[i + 1]] += 1

        i += 2

    sortedTable = {k: v for k, v in sorted(teamPoints.items(), reverse=True, key=lambda item: item[1])}

    for i in sortedTable:
        print(str(i) + ": " + str(sortedTable[i]))
    print("\n\n")


# Simulates the league an 'x' amount of times
def Simulate_Multiple_Seasons(matchStats_df, Season2019_matchStats_df, features_map, goal_model, st_dev_map):
    seasons = 10
    teamPoints = {team: 0 for team in TEAMS}

    for i in range(seasons):

        Season2019_predictions = []

        for row in Season2019_matchStats_df.iterrows():
            teamStats = row[1]
            simulated_result = Simulate_Game(teamStats, matchStats_df, features_map, goal_model, st_dev_map)
            Season2019_predictions.append(simulated_result)

        Season2019_predictions_df = pd.DataFrame(data=Season2019_predictions)
        teamPoints = Calculate_Team_Points(Season2019_predictions_df, teamPoints, Season2019_matchStats_df.iloc[:, 0])

    for i in teamPoints:
        teamPoints[i] = teamPoints[i] / 10
        print(str(i) + ": " + str(teamPoints[i]))


def CalculateFeaturesAndStdDev(matchStats_df):
    # ************************************************************************************
    # The Array below represents the list of features we want to predict for each match; *
    # they represent the stats of each team                                              *
    # 2 - Shots                                                                          *
    # 3 - Shots on target                                                                *
    # 4 - Fouls                                                                          *
    # 5 - Corners                                                                        *
    # 6 - Yellow Cards                                                                   *
    # 7 - Red Cards                                                                      *
    # ************************************************************************************
    featureArray = [2, 3, 4, 5, 6, 7]

    features_map = {2: LinearRegression(),  # shots
                    3: LinearRegression(),  # shotsOnTarget
                    4: LinearRegression(),  # fouls
                    5: LinearRegression(),  # corners
                    6: LinearRegression(),  # yellowCards
                    7: LinearRegression()  # redCards
                    }

    for i in featureArray:
        features_map[i] = Stats_Model(matchStats_df, LinearRegression(), i)

    for i in featureArray:
        st_dev_map = {i: np.std(matchStats_df[i]) for i in features_map.keys()}

    return features_map, st_dev_map


# Predicts (& returns) Season 2019
def Predict_Season2019(homeFeatures, awayFeatures, matchStats_df, features_map, goal_model, st_dev_map):
    Season2019_df = GetCurrentSeasonData()
    Season2019_matchStats = []

    for index, row in Season2019_df.iterrows():
        homeTeamStats = [row[item] for item in homeFeatures] + [1]
        awayTeamStats = [row[item] for item in awayFeatures] + [0]
        Season2019_matchStats.append(homeTeamStats)
        Season2019_matchStats.append(awayTeamStats)

    Season2019_matchStats_df = pd.DataFrame(Season2019_matchStats)

    # This list will store the predicted outcomes of each match (along with their stats)
    # for the 2019 season
    Season2019_predictions = []

    for row in Season2019_matchStats_df.iterrows():
        teamStats = row[1]
        simulated_result = Simulate_Game(teamStats, matchStats_df, features_map, goal_model, st_dev_map)
        Season2019_predictions.append(simulated_result)

    Season2019_predictions_df = pd.DataFrame(data=Season2019_predictions)

    return Season2019_df, Season2019_matchStats_df, Season2019_predictions_df


def generatedstatstomatrix(Season2019_predictions_df):
    Matrix = np.empty((380, 12), dtype=object)

    i = 0
    j = 0
    k = 0
    m = 0
    while i < 380:
        while j < 12:
            Matrix[i][j] = Season2019_predictions_df.iloc[k, m]
            Matrix[i][j + 1] = Season2019_predictions_df.iloc[k + 1, m]
            j += 2
            m += 1
        j = 0
        m = 0
        k += 2
        i += 1
    Matrix = pd.DataFrame.from_records(Matrix)
    Matrix = Matrix.iloc[:, [0, 1, 2, 3, 8, 10]]

    return Matrix


def main():
    knum = 12
    init_Data = Clean_and_Get_Data()
    masterData = copy.deepcopy(init_Data)

    # drop FTR and team names (because these are not numeric values)
    del masterData['FTR']
    del masterData['HomeTeam']
    del masterData['AwayTeam']
    # print(masterData)

    np_Data = masterData.loc[:, NonPredictive_FEATURES]
    x = np_Data.values
    y = masterData['Outcome'].values
    # features = np_Data.columns
    ChiSquared(knum, x, y)
    FeatureImportance(knum, x, y)

    # These features are features that we care about, and will use them in predicting
    # the scores of matches played in the 2019 season
    homeFeatures = ['HomeTeam', 'FTHG', 'HS', 'HST', 'HF', 'HC', 'HY', 'HR']
    awayFeatures = ['AwayTeam', 'FTAG', 'AS', 'AST', 'AF', 'AC', 'AY', 'AR']

    # This list will store the match stats for EACH team in EACH match;
    # the matches are from 2000 - 2018
    matchStats = []

    for index, row in init_Data.iterrows():
        homeTeamStats = [row[item] for item in homeFeatures] + [1]  # 1 = home
        awayTeamStats = [row[item] for item in awayFeatures] + [0]  # 0 = away
        matchStats.append(homeTeamStats)
        matchStats.append(awayTeamStats)

    matchStats_df = pd.DataFrame(matchStats)
    matchStats_df_CLEAN = copy.deepcopy(matchStats_df)
    matchStats_df_CLEAN = matchStats_df_CLEAN.drop([0, 1, 8],
                                                   axis=1)  # Gets rid of the 'team name', 'goals scored', and 'home/away' columns

    # 2nd argument to the function call is the number of goals scored by each team
    print(matchStats_df)
    goal_model = ExpectedGoals_Model(matchStats_df_CLEAN, matchStats_df[matchStats_df.columns[1:2]],
                                     GradientBoostingRegressor(loss='huber', max_depth=7))

    features_map, st_dev_map = CalculateFeaturesAndStdDev(matchStats_df)

    Season2019_df, Season2019_matchStats_df, Season2019_predictions_df = Predict_Season2019(homeFeatures, awayFeatures,
                                                                                            matchStats_df, features_map,
                                                                                            goal_model, st_dev_map)

    Calculate_Team_Points(Season2019_predictions_df, Season2019_matchStats_df.iloc[:, 0])

    Season2019_pre = generatedstatstomatrix(Season2019_predictions_df)

    print("Actual 2019 stats")
    SVM(masterData, Season2019_df, Season2019_pre, 0)
    print("\nGenerated 2019 stats ")
    SVM(masterData, Season2019_df, Season2019_pre, 1)
    print("\n")
    print("Actual 2019 stats")
    LogReg(masterData, Season2019_df, 0)
    print("\nGenerated 2019 stats ")
    LogReg(masterData, Season2019_pre, 1)


if __name__ == "__main__":
    main()