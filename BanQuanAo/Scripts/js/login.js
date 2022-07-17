const form = document.querySelector("#form");
const username = document.querySelector("#username");
const password = document.querySelector("#password");

form.addEventListener("submit", (e) => {
  e.preventDefault();

  checkInput();
});
function checkInput() {
  const usernameValue = username.value.trim();

  const passwordValue = password.value.trim();

  if (usernameValue === "") {
    setErrorFor(username, "Tên đăng nhập không thể để trống");
  } else {
    setSuccessFor(username);
  }

  if (passwordValue === "") {
    setErrorFor(password, "Mật khẩu không thể để trống");
  } else {
    setSuccessFor(password);
  }
}
function setErrorFor(input, message) {
  const formControl = input.parentElement; //form-control
  const small = formControl.querySelector("small");
  // add error message inside small
  small.innerText = message;
  // add error class
  formControl.className = "form-control error";
}
function setSuccessFor(input) {
  const formControl = input.parentElement;
  formControl.className = "form-control success";
}
