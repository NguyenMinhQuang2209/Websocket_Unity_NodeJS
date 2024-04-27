const user = require('./user.router');
function router(app){
    app.use('/auth',user);
}
module.exports = router;