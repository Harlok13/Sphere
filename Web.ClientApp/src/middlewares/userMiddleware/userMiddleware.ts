export const userMiddleware = store => next => action => {
    const user = JSON.parse(localStorage.getItem("user"));
    const userStatistic = JSON.parse(user.userStatistic);

    const modifiedAction = {
        ...action,
        payload: {
            ...action.payload,
            user: user,
            userStatistic: userStatistic
        }
    };

    console.log(user, "middleware");

    return next(modifiedAction);
};

