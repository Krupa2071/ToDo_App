<template>
  <div class="todo-form">
    <h2>Add New Todo</h2>
    <form @submit.prevent="handleSubmit">
      <div class="form-group">
        <label for="title">Title</label>
        <input 
          type="text" 
          id="title" 
          v-model="todo.title" 
          required
          placeholder="Enter todo title"
        />
      </div>
      
      <div class="form-group">
        <label for="description">Description</label>
        <textarea 
          id="description" 
          v-model="todo.description" 
          placeholder="Enter todo description"
        ></textarea>
      </div>
      
      <button type="submit" class="btn-submit">Add Todo</button>
    </form>
  </div>
</template>

<script>
export default {
  name: 'TodoForm',
  data() {
    return {
      todo: {
        title: '',
        description: '',
        isCompleted: false
      }
    }
  },
  methods: {
    handleSubmit() {
      this.$emit('todo-added', { ...this.todo })
      
      // Reset form
      this.todo = {
        title: '',
        description: '',
        isCompleted: false
      }
    }
  }
}
</script>

<style scoped>
.todo-form {
  background-color: var(--card-background);
  padding: 20px;
  border-radius: 8px;
  box-shadow: var(--shadow);
  transition: background-color 0.3s ease, box-shadow 0.3s ease;
}

.form-group {
  margin-bottom: 15px;
}

label {
  display: block;
  margin-bottom: 5px;
  font-weight: bold;
  color: var(--text-color);
  transition: color 0.3s ease;
}

input, textarea {
  width: 100%;
  padding: 10px;
  border: 1px solid var(--border-color);
  border-radius: 4px;
  font-size: 16px;
  transition: all 0.3s ease;
}

textarea {
  height: 100px;
  resize: vertical;
}

.btn-submit {
  background-color: var(--primary-color);
  color: white;
  border: none;
  padding: 10px 15px;
  border-radius: 4px;
  cursor: pointer;
  font-size: 16px;
  transition: background-color 0.3s ease;
}

.btn-submit:hover {
  background-color: var(--primary-dark);
}
</style>