# spec/tests_spec.rb

require 'net/http'
require 'uri'
require 'json'

BASE_URL = 'https://restful-booker.herokuapp.com'
VALID_USERNAME = 'admin'
VALID_PASSWORD = 'password123'

def authPost(body)
  uri = URI.parse("#{BASE_URL}/auth")
  http = Net::HTTP.new(uri.host, uri.port)
  http.use_ssl = true
  request = Net::HTTP::Post.new(uri.request_uri)
  request['Content-Type'] = 'application/json'
  request.body = body.to_json if body
  http.request(request)
end

describe '/Auth' do
  it 'POST /auth with valid credentials' do
    response = authPost({ username: "#{VALID_USERNAME}", password: "#{VALID_PASSWORD}" })
    json_response = JSON.parse(response.body)

    expect(response.code.to_i).to eq(200)
    expect(response['Content-Type']).to include('application/json')   
    expect(json_response).to include('token')
  end

  it 'POST /auth with invalid username credentials' do
    response = authPost({ username: "doesnotexist", password: "#{VALID_PASSWORD}" })
    json_response = JSON.parse(response.body)

    expect(response.code.to_i).to eq(200)
    expect(response['Content-Type']).to include('application/json')   
    expect(json_response).to include('reason')
    expect(json_response['reason']).to eq('Bad credentials')
  end

  it 'POST /auth with invalid password credentials' do
    response = authPost({ username: "#{VALID_USERNAME}", password: "badpassword" })
    json_response = JSON.parse(response.body)

    expect(response.code.to_i).to eq(200)
    expect(response['Content-Type']).to include('application/json')   
    expect(json_response).to include('reason')
    expect(json_response['reason']).to eq('Bad credentials')
  end

  it 'POST /auth with no credentials' do
    response = authPost({ })
    json_response = JSON.parse(response.body)

    expect(response.code.to_i).to eq(200)
    expect(response['Content-Type']).to include('application/json')   
    expect(json_response).to include('reason')
    expect(json_response['reason']).to eq('Bad credentials')
  end

  it 'POST /auth with missing username credentials' do
    response = authPost({ password: "#{VALID_PASSWORD}" })
    json_response = JSON.parse(response.body)

    expect(response.code.to_i).to eq(200)
    expect(response['Content-Type']).to include('application/json')   
    expect(json_response).to include('reason')
    expect(json_response['reason']).to eq('Bad credentials')
  end

  it 'POST /auth with missing password credentials' do
    response = authPost({ username: "#{VALID_USERNAME}" })
    json_response = JSON.parse(response.body)

    expect(response.code.to_i).to eq(200)
    expect(response['Content-Type']).to include('application/json')   
    expect(json_response).to include('reason')
    expect(json_response['reason']).to eq('Bad credentials')
  end
end