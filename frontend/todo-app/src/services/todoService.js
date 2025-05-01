import axios from 'axios'

const API_URL = 'http://localhost:5204/api/todos'

export default {
  async getAllTodos() {
    const response = await axios.get(API_URL)
    return response.data
  },
  
  async searchTodos(term) {
    const response = await axios.get(`${API_URL}/search?term=${encodeURIComponent(term)}`)
    return response.data
  },
  
  async createTodo(todo) {
    const response = await axios.post(API_URL, todo)
    return response.data
  },
  
  async updateTodo(todo) {
    await axios.put(`${API_URL}/${todo.id}`, todo)
    return todo
  },
  
  async deleteTodo(id) {
    await axios.delete(`${API_URL}/${id}`)
    return id
  },
  
  async setProvider(providerType) {
    const response = await axios.post(`${API_URL}/provider/${providerType}`)
    return response.data
  }
}