import { steamLogin } from "../../managers/steamAuthManager";
// import steamLogo from "../../assets/images/steam-logo.png";

export default function SteamLoginButton() {
  return (
    <button
      onClick={steamLogin}
      style={{
        display: "flex",
        alignItems: "center",
        backgroundColor: "#171a21",
        color: "white",
        border: "none",
        padding: "10px 20px",
        borderRadius: "5px",
        cursor: "pointer",
        fontSize: "16px",
      }}
    >
      <i className="bi bi-steam me-2"></i>
      Login with Steam
    </button>
  );
}
