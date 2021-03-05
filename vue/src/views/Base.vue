<template>
  <div>
      
  </div>
</template>

<script>
export default {
    mounted() {
        this.$router.push("/MyDrive/" + encodeURIComponent("/" + localStorage.Username));
    }

}
</script>

<style>

</style>