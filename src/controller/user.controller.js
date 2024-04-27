const User = require('../module/user.module');
class UserController{
    async register(req,res){
        try{
            const {username,password} = req.body;
            console.log({username,password});
            const user = await User.findOne({username});
            if(user){
                return res.status(400).json({msg:"Lỗi server"});
            }
            const newUser = new User({
                username,
                password
            });
            await newUser.save();
            return res.status(200).json({msg:"Tạo tài khoản thành công."});
        }
        catch(err){
            return res.status(500).json({msg:err.toString()});
        }
    }
    async login(req,res){
        try{
            const user = await User.findOne({username,password});
            console.log({username,password});
            if(user){
                return res.status(200).json({msg:"Đăng nhập thành công."});
            }
            return res.status(400).json({msg:"Lỗi server"});
        } catch(err){
            return res.status(500).json({msg:err.toString()});
        }
    }

    async getUsers(req,res){
        try{
            const users = await User.find().select("username password");
            return res.status(200).json([...users]);
        }
        catch(err){
            return res.status(500).json({msg:err.toString()});
        }
    }
}

module.exports = new UserController();