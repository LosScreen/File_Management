
<template>
  <section id="cover" class="min-vh-100" style="background: green">
    <div id="cover-caption">
      <div class="container">
        <div class="row text-white" style="justify-content: center;">
          <div
            class="col-11 col-xl-5 col-lg-6 col-md-8 col-sm-10 text-center form p-4 div_left"
            style=""
          >
            <img src="@/assets/logo.png" />
          </div>
          <div
            class="col-11 col-xl-5 col-lg-6 col-md-8 col-sm-10 text-center form p-4 div_right"
            style=""
          >
          <form class="form" style="background-color:none;" v-on:submit.prevent="">
            <h1 class="" style="color: black">Create Account</h1>
            <div class="px-2">
              <!-- <form action="" class="justify-content-center"> -->
              <div class="form-group row mb-2" style="">
                <div
                  class="col"
                  style="
                    background-color: #d4d4d4;
                    border-radius: 25px;
                    height: 40px;
                  "
                >
                  <i
                    class="far fa-user"
                    style="color: gray; margin-right: 10px"
                  ></i>
                  <input
                    style="margin: 0 0 10px 0"
                    class="text_box"
                    v-model="UserData.userName"
                    placeholder="Username" 
                  />
                </div>
              </div>
              <div class="form-group row mb-3" style="">
                <div
                  class="col"
                  style="
                    background-color: #d4d4d4;
                    border-radius: 25px;
                    height: 40px;
                  "
                >
                  <i
                    class="fas fa-lock"
                    style="color: gray; margin-right: 10px"
                  ></i>
                  <input
                    type="password"
                    style="margin: 0 0 10px 0"
                    class="text_box"
                    v-model="UserData.passWord"
                    placeholder="Password"
                    
                  />
                </div>
              </div>
              <div class="row">
                <button @click="Register()" class="botton_Register_Main">Register</button>
                 <a class="botton_Login_Sup" style="color:gray; margin-top:10px" @click="Login()">Login</a>
              </div>
              <!-- </form> -->
            </div>
            </form>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script>
export default {
  methods: {
    Login() {
      this.$router.push("/Login");
    },
    Register() {
      if (this.UserData.userName == null) {
        alert("Username is null");
      } else if (this.UserData.passWord == null) {
        alert("Password is null");
      } else {
        this.$axios
          .post("User/checkRegister", this.UserData)
          .then((res) => {
            console.log(res.data);
            if (res.data != "notEmpty") {
              this.$axios
                .post("User/Register", this.UserData)
                .then(() => {
                  alert("Resgiter is complete");
                })
                .catch(function (error) {
                  console.log(error);
                });
            }
            else {
              alert("Can't use this Username.");
            }
          })
          .catch(function (error) {
            console.log(error);
          });
      }
    },
  },
  data() {
    return {
      UserData: {
        userName: undefined,
        passWord: undefined,
      },
    };
  },
};
</script>

<style>
@media only screen and (max-width: 991px) {
  .div_left {
    border-top-left-radius: 10px;
    border-top-right-radius: 10px;
    border-bottom-left-radius: 0px!important;
    border-bottom-right-radius: 0px!important;
  }
  .div_right {
    border-top-left-radius: 0px!important;
    border-top-right-radius: 0px!important;
    border-bottom-left-radius: 10px;
    border-bottom-right-radius: 10px;
  }
}
.botton_Login_Sup{
  cursor: pointer;
}
.div_left {
  padding:100px 0px!important;
  background-color: white;
  border-bottom-left-radius: 10px;
  border-top-left-radius: 10px;
}
.div_right {
  padding:100px 30px!important;
  background-color: white;
  border-bottom-right-radius: 10px;
  border-top-right-radius: 10px;
}
.text_box {
  border: none;
  height: 100%;
  width: 90%;
  border-radius: 25px;
  background: none;
}
.text_box:focus {
  outline: 0;
}
.botton_Register_Main {
  width: 100%;
  height: 40px;
  border-radius: 25px;
  background-color: #63c67b;
  color: white;
  border: none;

  transition: all 0.3s;
}
.botton_Register_Main:hover {
  background-color: #2eaa4d;
}
.botton_Register_Main:focus {
  outline: 0;
}
.botton_Register_Main:active {
  background-color: #0f501f;
}
#cover {
  background: url("") center center no-repeat;
  background-size: cover;
  height: 100%;
  text-align: center;
  display: flex;
  align-items: center;
  position: relative;
}

#cover-caption {
  width: 100%;
  position: relative;
  z-index: 1;
}

/* only used for background overlay not needed for centering */
form:before {
  content: "";
  height: 100%;
  left: 0;
  position: absolute;
  top: 0;
  width: 100%;
  background-color: rgba(255, 255, 255, 0.3);
  z-index: -1;
  border-radius: 10px;
}
</style>