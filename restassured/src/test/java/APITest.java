import org.testng.annotations.BeforeClass;
import org.testng.annotations.Test;
import io.restassured.RestAssured;
import io.restassured.response.Response;
import static org.testng.Assert.assertNotNull;
import static org.testng.Assert.assertEquals;
import static io.restassured.RestAssured.given;

public class APITest {
    String valid_username = "admin";
    String valid_password = "password123";

    @BeforeClass
    public void setUp() {
        RestAssured.baseURI = "https://restful-booker.herokuapp.com";
    }

    @Test
    public void Post_Auth_With_Valid_Credientials() {
        String requestBody = "{ \"username\": \"" + valid_username + "\", \"password\": \"" + valid_password + "\" }";
        Response response = APIAuth(requestBody);
        response.then().statusCode(200);
        response.then().log().all();
        String token = response.jsonPath().getString("token");
        assertNotNull(token, "Token not found in the response");
    }    

    @Test
    public void Post_Auth_With_Invalid_Username_Credientials() {
        String requestBody = "{ \"username\": \"doesnotexist\", \"password\": \"" + valid_password + "\" }";
        Response response = APIAuth(requestBody);
        response.then().statusCode(200);
        response.then().log().all();
        String reason = response.jsonPath().getString("reason");
        assertEquals(reason, "Bad credentials", "Unexpected reason for authentication failure");
    }
 
    @Test
    public void Post_Auth_With_Invalid_Password_Credientials() {
        String requestBody = "{ \"username\": \"" + valid_username + "\", \"password\": \"badpassword\" }";
        Response response = APIAuth(requestBody);
        response.then().statusCode(200);
        response.then().log().all();
        String reason = response.jsonPath().getString("reason");
        assertEquals(reason, "Bad credentials", "Unexpected reason for authentication failure");
    }
 
    @Test
    public void Post_Auth_With_No_Credientials() {
        String requestBody = "{  }";
        Response response = APIAuth(requestBody);
        response.then().statusCode(200);
        response.then().log().all();
        String reason = response.jsonPath().getString("reason");
        assertEquals(reason, "Bad credentials", "Unexpected reason for authentication failure");
    }

    @Test
    public void Post_Auth_With_Missing_Username_Credientials() {
        String requestBody = "{ \"password\": \"" + valid_password + "\" }";
        Response response = APIAuth(requestBody);
        response.then().statusCode(200);
        response.then().log().all();
        String reason = response.jsonPath().getString("reason");
        assertEquals(reason, "Bad credentials", "Unexpected reason for authentication failure");
    }

    @Test
    public void Post_Auth_With_Missing_Password_Credientials() {
        String requestBody = "{ \"username\": \"" + valid_username + "\" }";
        Response response = APIAuth(requestBody);
        response.then().statusCode(200);
        response.then().log().all();
        String reason = response.jsonPath().getString("reason");
        assertEquals(reason, "Bad credentials", "Unexpected reason for authentication failure");
    }

    private Response APIAuth(String requestBody) {
        return given()
                .contentType("application/json")
                .body(requestBody)
            .when()
                .post("/auth");
    }
}