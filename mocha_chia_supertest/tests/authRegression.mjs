import supertest from 'supertest';
const request = supertest('https://restful-booker.herokuapp.com');
import * as chai from 'chai';
const { assert } = chai;

var validUsername = 'admin';
var validPassword = 'password123';

it('POST /auth with valid credentials', () => {
  const data = {
      username: validUsername,
      password: validPassword,
   };
  return request
   .post ('/auth')
   .send(data)
   .expect(200)
   .then((res) => {
    assert.isNotEmpty(res.body);
    assert.isObject(res.body);
    assert.hasAnyKeys(res.body, 'token');
    });
});

it('POST /auth with invalid username credentials', () => {
  const data = {
      username: 'doesnotexist',
      password: validPassword,
   };
  return request
   .post ('/auth')
   .send(data)
   .expect(200)
   .then((res) => {
    assert.isNotEmpty(res.body);
    assert.isObject(res.body);
    assert.hasAnyKeys(res.body, 'reason');
    assert.equal(res.body.reason,'Bad credentials');
    });
});

it('POST /auth with invalid password credentials', () => {
  const data = {
      username: validUsername,
      password: 'badpassword',
   };
  return request
   .post ('/auth')
   .send(data)
   .expect(200)
   .then((res) => {
    assert.isNotEmpty(res.body);
    assert.isObject(res.body);
    assert.hasAnyKeys(res.body, 'reason');
    assert.equal(res.body.reason,'Bad credentials');
    });
});

it('POST /auth with no credentials', () => {
  const data = {};
  return request
   .post ('/auth')
   .send(data)
   .expect(200)
   .then((res) => {
    assert.isNotEmpty(res.body);
    assert.isObject(res.body);
    assert.hasAnyKeys(res.body, 'reason');
    assert.equal(res.body.reason,'Bad credentials');
    });
});

it('POST /auth with missing username credentials', () => {
  const data = {
    password: validPassword
   };
  return request
   .post ('/auth')
   .send(data)
   .expect(200)
   .then((res) => {
    assert.isNotEmpty(res.body);
    assert.isObject(res.body);
    assert.hasAnyKeys(res.body, 'reason');
    assert.equal(res.body.reason,'Bad credentials');
    });
});

it('POST /auth with missing password credentials', () => {
  const data = {
      username: validUsername
   };
  return request
   .post ('/auth')
   .send(data)
   .expect(200)
   .then((res) => {
    assert.isNotEmpty(res.body);
    assert.isObject(res.body);
    assert.hasAnyKeys(res.body, 'reason');
    assert.equal(res.body.reason,'Bad credentials');
    });
});