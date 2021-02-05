
<template>
  <section id="cover" class="min-vh-100">
    <div id="cover-caption">
      <div class="container">
        <div class="row text-white">
          <div
            class="col-xl-5 col-lg-6 col-md-8 col-sm-10 mx-auto text-center form p-4"
            style="background-color: yellow"
          >
            <h1 class="" style="color: black">Login Form</h1>
            <div class="px-2">
              <!-- <form action="" class="justify-content-center"> -->
              <div class="form-group">
                <input
                  style="margin: 0 0 10px 0"
                  class="form-control"
                  v-model="UserData.userName"
                  placeholder="Username"
                />
              </div>
              <div class="form-group">
                <input
                type="password"
                  style="margin: 0 0 10px 0"
                  class="form-control"
                  v-model="UserData.passWord"
                  placeholder="Password"
                />
              </div>
              <button @click="Register()" class="btn btn-primary btn-lg">
                Login
              </button>
              <!-- </form> -->
            </div>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script>
export default {
  methods: {
    Register() {
      if (this.UserData.userName == null) {
        alert("Username is null");
      } else if (this.UserData.passWord == null) {
        alert("Password is null");
      } else {
        this.$axios
          .post("User/login", this.UserData)
          .then((res) => {
            if (res.data == "empty") {
              alert("Username or Password is Not correct");
            } else {
              console.log(res.data);
            }
            //   console.log("res");
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
        userName: null,
        passWord: null,
      },
    };
  },
};
</script>

<style>
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